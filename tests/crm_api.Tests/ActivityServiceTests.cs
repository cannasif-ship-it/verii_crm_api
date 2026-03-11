using AutoMapper;
using crm_api.Data;
using crm_api.DTOs;
using crm_api.Infrastructure;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.Repositories;
using crm_api.Services;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace crm_api.Tests;

public class ActivityServiceTests
{
    private sealed class FakeLocalizationService : ILocalizationService
    {
        public string GetLocalizedString(string key) => key;
        public string GetLocalizedString(string key, params object[] arguments) => key;
    }

    private static async Task<(ActivityService Service, CmsDbContext Context)> BuildServiceAsync(
        Action<CmsDbContext>? seed = null)
    {
        var dbName = $"crm-activity-tests-{Guid.NewGuid()}";
        var options = new DbContextOptionsBuilder<CmsDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var context = new CmsDbContext(options);
        seed?.Invoke(context);
        await context.SaveChangesAsync();

        var httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext()
        };

        var activityRepo = new GenericRepository<Activity>(context, httpContextAccessor);
        var activityTypeRepo = new GenericRepository<ActivityType>(context, httpContextAccessor);
        var userRepo = new GenericRepository<User>(context, httpContextAccessor);
        var contactRepo = new GenericRepository<Contact>(context, httpContextAccessor);
        var customerRepo = new GenericRepository<Customer>(context, httpContextAccessor);

        var mapper = new Mock<IMapper>();
        mapper
            .Setup(x => x.Map<Activity>(It.IsAny<CreateActivityDto>()))
            .Returns((CreateActivityDto dto) => new Activity
            {
                Subject = dto.Subject,
                Description = dto.Description,
                ActivityTypeId = dto.ActivityTypeId,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                IsAllDay = dto.IsAllDay,
                Status = dto.Status,
                Priority = dto.Priority,
                AssignedUserId = dto.AssignedUserId,
                ContactId = dto.ContactId,
                PotentialCustomerId = dto.PotentialCustomerId,
                ErpCustomerCode = dto.ErpCustomerCode,
                Reminders = dto.Reminders.Select(r => new ActivityReminder
                {
                    OffsetMinutes = r.OffsetMinutes,
                    Channel = r.Channel,
                    Status = ReminderStatus.Pending
                }).ToList()
            });
        mapper
            .Setup(x => x.Map<ActivityDto>(It.IsAny<Activity>()))
            .Returns((Activity activity) => new ActivityDto
            {
                Id = activity.Id,
                Subject = activity.Subject,
                Description = activity.Description,
                ActivityTypeId = activity.ActivityTypeId,
                StartDateTime = activity.StartDateTime,
                EndDateTime = activity.EndDateTime,
                IsAllDay = activity.IsAllDay,
                Status = activity.Status,
                Priority = activity.Priority,
                AssignedUserId = activity.AssignedUserId,
                ContactId = activity.ContactId,
                PotentialCustomerId = activity.PotentialCustomerId,
                ErpCustomerCode = activity.ErpCustomerCode,
                ActivityType = new ActivityTypeGetDto { Id = activity.ActivityTypeId, Name = activity.ActivityType?.Name ?? string.Empty },
                AssignedUser = new UserDto { Id = activity.AssignedUserId },
                Reminders = activity.Reminders.Select(r => new ActivityReminderDto
                {
                    ActivityId = activity.Id,
                    OffsetMinutes = r.OffsetMinutes,
                    Channel = r.Channel,
                    Status = r.Status
                }).ToList()
            });

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(x => x.Activities).Returns(activityRepo);
        uow.SetupGet(x => x.ActivityTypes).Returns(activityTypeRepo);
        uow.SetupGet(x => x.Users).Returns(userRepo);
        uow.SetupGet(x => x.Contacts).Returns(contactRepo);
        uow.SetupGet(x => x.Customers).Returns(customerRepo);
        uow.Setup(x => x.SaveChangesAsync()).Returns(() => context.SaveChangesAsync());
        uow.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
        uow.Setup(x => x.CommitTransactionAsync()).Returns(Task.CompletedTask);
        uow.Setup(x => x.RollbackTransactionAsync()).Returns(Task.CompletedTask);

        var localization = new FakeLocalizationService();
        var googleCalendar = new Mock<IGoogleCalendarService>();
        var oauthSettings = new Mock<ITenantGoogleOAuthSettingsService>();
        oauthSettings
            .Setup(x => x.GetRuntimeSettingsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TenantGoogleOAuthRuntimeSettings?)null);

        var userContext = new Mock<IUserContextService>();
        userContext.Setup(x => x.GetCurrentUserId()).Returns(1);

        var service = new ActivityService(
            uow.Object,
            mapper.Object,
            localization,
            googleCalendar.Object,
            oauthSettings.Object,
            userContext.Object);

        return (service, context);
    }

    [Fact]
    public async Task CreateActivityAsync_ShouldReturnSpecificError_WhenActivityTypeDoesNotExist()
    {
        var existingUser = new User
        {
            Username = "test.user",
            Email = "test@example.com",
            PasswordHash = "hash",
            IsDeleted = false
        };

        var (service, _) = await BuildServiceAsync(db =>
        {
            db.Users.Add(existingUser);
        });

        var dto = new CreateActivityDto
        {
            Subject = "Aktivite",
            ActivityTypeId = 999,
            StartDateTime = DateTime.UtcNow,
            EndDateTime = DateTime.UtcNow.AddHours(1),
            AssignedUserId = existingUser.Id
        };

        var result = await service.CreateActivityAsync(dto);

        Assert.False(result.Success);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.Equal("ActivityService.InvalidActivityType", result.ExceptionMessage);
    }

    [Fact]
    public async Task CreateActivityAsync_ShouldCreateActivity_WhenPayloadIsValid()
    {
        var existingType = new ActivityType
        {
            Name = "Ziyaret",
            IsDeleted = false
        };

        var existingUser = new User
        {
            Username = "test.user",
            Email = "test@example.com",
            PasswordHash = "hash",
            IsDeleted = false
        };

        var (service, context) = await BuildServiceAsync(db =>
        {
            db.ActivityTypes.Add(existingType);
            db.Users.Add(existingUser);
        });

        var start = DateTime.UtcNow;
        var dto = new CreateActivityDto
        {
            Subject = "Musteri ziyareti",
            Description = "Planlandi",
            ActivityTypeId = existingType.Id,
            StartDateTime = start,
            EndDateTime = start.AddHours(1),
            AssignedUserId = existingUser.Id,
            Reminders = new List<CreateActivityReminderDto>
            {
                new() { OffsetMinutes = 15, Channel = ReminderChannel.InApp }
            }
        };

        var result = await service.CreateActivityAsync(dto);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.True(result.Data!.Id > 0);

        var created = await context.Activities.Include(x => x.Reminders).FirstOrDefaultAsync(x => x.Id == result.Data.Id);
        Assert.NotNull(created);
        Assert.Equal(dto.Subject, created!.Subject);
        Assert.Equal(existingType.Id, created.ActivityTypeId);
        Assert.Equal(existingUser.Id, created.AssignedUserId);
        Assert.Single(created.Reminders);
    }
}
