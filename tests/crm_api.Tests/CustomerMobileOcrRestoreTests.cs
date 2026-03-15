using System.Security.Claims;
using AutoMapper;
using crm_api.Data;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.Repositories;
using crm_api.Services;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace crm_api.Tests;

public class CustomerMobileOcrRestoreTests
{
    private sealed class FakeLocalizationService : ILocalizationService
    {
        public string GetLocalizedString(string key)
        {
            return key switch
            {
                "General.Unknown" => "Bilinmeyen",
                "CustomerService.CustomerCreated" => "Müşteri başarı ile oluşturuldu",
                "CustomerService.MobileOcrDuplicateContact" => "Bu kişi sisteme daha önce eklenmiştir.",
                "CustomerService.ConflictingCustomerMatches" => "Birden fazla müşteri eşleşmesi bulundu.",
                _ => key
            };
        }

        public string GetLocalizedString(string key, params object[] arguments)
        {
            return $"{GetLocalizedString(key)}: {string.Join(", ", arguments)}";
        }
    }

    private static CustomerCreateFromMobileDto BuildRequest(
        string companyName = "Windoform",
        string? email = "a.yuksel@windoform.com.tr",
        string? phone = "05324254821",
        string? phone2 = "02328547000",
        string contactName = "Aydin Yuksel",
        string? title = "Genel Müdür")
    {
        return new CustomerCreateFromMobileDto
        {
            Name = companyName,
            ContactName = contactName,
            Email = email,
            Phone = phone,
            Phone2 = phone2,
            Title = title,
            Address = "Fabrika: Kazım Karabekir Mh.",
            Notes = "OCR test",
            BranchCode = 1,
            BusinessUnitCode = 1
        };
    }

    private static async Task<(CustomerService Service, CmsDbContext Context)> BuildServiceAsync(
        Action<CmsDbContext>? seed = null)
    {
        var dbName = $"crm-ocr-tests-{Guid.NewGuid()}";
        var options = new DbContextOptionsBuilder<CmsDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var context = new CmsDbContext(options);
        seed?.Invoke(context);
        await context.SaveChangesAsync();

        var httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1")
                    }, "test"))
            }
        };

        var customerRepo = new GenericRepository<Customer>(context, httpContextAccessor);
        var contactRepo = new GenericRepository<Contact>(context, httpContextAccessor);
        var titleRepo = new GenericRepository<Title>(context, httpContextAccessor);
        var imageRepo = new GenericRepository<CustomerImage>(context, httpContextAccessor);
        var countryRepo = new GenericRepository<Country>(context, httpContextAccessor);
        var cityRepo = new GenericRepository<City>(context, httpContextAccessor);
        var districtRepo = new GenericRepository<District>(context, httpContextAccessor);

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(x => x.Customers).Returns(customerRepo);
        uow.SetupGet(x => x.Contacts).Returns(contactRepo);
        uow.SetupGet(x => x.Titles).Returns(titleRepo);
        uow.SetupGet(x => x.CustomerImages).Returns(imageRepo);
        uow.SetupGet(x => x.Countries).Returns(countryRepo);
        uow.SetupGet(x => x.Cities).Returns(cityRepo);
        uow.SetupGet(x => x.Districts).Returns(districtRepo);
        uow.Setup(x => x.SaveChangesAsync()).Returns(() => context.SaveChangesAsync());
        uow.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
        uow.Setup(x => x.CommitTransactionAsync()).Returns(Task.CompletedTask);
        uow.Setup(x => x.RollbackTransactionAsync()).Returns(Task.CompletedTask);

        var mapper = new Mock<IMapper>();
        var localization = new FakeLocalizationService();
        var erp = new Mock<IErpService>();
        var geocoding = new Mock<IGeocodingService>();
        geocoding.Setup(x => x.GeocodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(((decimal Latitude, decimal Longitude)?)null);

        var fileUpload = new Mock<IFileUploadService>();
        fileUpload.Setup(x => x.UploadCustomerImageAsync(It.IsAny<IFormFile>(), It.IsAny<long>()))
            .ReturnsAsync(ApiResponse<string>.ErrorResult("Test.UploadDisabled"));

        var service = new CustomerService(
            uow.Object,
            mapper.Object,
            localization,
            erp.Object,
            NullLogger<CustomerService>.Instance,
            httpContextAccessor,
            geocoding.Object,
            fileUpload.Object);

        return (service, context);
    }

    private static Customer SeedCustomer(
        string companyName,
        bool isDeleted = false,
        string? email = null,
        string? phone1 = null,
        short branchCode = 1)
    {
        return new Customer
        {
            CustomerName = companyName,
            Email = email,
            Phone1 = phone1,
            IsDeleted = isDeleted,
            DeletedDate = isDeleted ? DateTime.UtcNow : null,
            BranchCode = branchCode,
            BusinessUnitCode = 1
        };
    }

    private static Contact SeedContact(
        long customerId,
        string fullName = "Aydin Yuksel",
        string? email = "a.yuksel@windoform.com.tr",
        string? mobile = "05324254821",
        string? phone = "02328547000",
        bool isDeleted = false,
        long? titleId = null)
    {
        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var firstName = parts.FirstOrDefault() ?? fullName;
        var lastName = parts.Length > 1 ? parts.Last() : firstName;
        var middleName = parts.Length > 2 ? string.Join(" ", parts.Skip(1).Take(parts.Length - 2)) : null;

        return new Contact
        {
            CustomerId = customerId,
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            FullName = fullName,
            Email = email,
            Mobile = mobile,
            Phone = phone,
            TitleId = titleId,
            IsDeleted = isDeleted,
            DeletedDate = isDeleted ? DateTime.UtcNow : null
        };
    }

    private static Title SeedTitle(string titleName, bool isDeleted = false)
    {
        return new Title
        {
            TitleName = titleName,
            IsDeleted = isDeleted,
            DeletedDate = isDeleted ? DateTime.UtcNow : null
        };
    }

    [Fact]
    public async Task Company_WhenMissing_CreatesNewCompany()
    {
        var (service, context) = await BuildServiceAsync();

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest());

        Assert.True(result.Success);
        Assert.Equal("Created", result.Data!.CustomerAction);
        Assert.Single(context.Customers.IgnoreQueryFilters());
    }

    [Fact]
    public async Task Company_WhenExisting_ReusesExistingId()
    {
        var existingCustomer = SeedCustomer("Windoform");
        var (service, _) = await BuildServiceAsync(db => db.Customers.Add(existingCustomer));

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest());

        Assert.True(result.Success);
        Assert.Equal(existingCustomer.Id, result.Data!.CustomerId);
        Assert.Equal("Reused", result.Data.CustomerAction);
    }

    [Fact]
    public async Task Company_WhenSoftDeleted_ReactivatesExistingCompany()
    {
        var deletedCustomer = SeedCustomer("Windoform", isDeleted: true);
        var (service, context) = await BuildServiceAsync(db => db.Customers.Add(deletedCustomer));

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest());

        Assert.True(result.Success);
        Assert.Equal("Reactivated", result.Data!.CustomerAction);
        var restored = await context.Customers.IgnoreQueryFilters().FirstAsync(x => x.Id == deletedCustomer.Id);
        Assert.False(restored.IsDeleted);
    }

    [Fact]
    public async Task Company_WhenSameOcrRunsTwice_DoesNotCreateDuplicateCompany()
    {
        var (service, context) = await BuildServiceAsync();
        var request = BuildRequest();

        var first = await service.CreateCustomerFromMobileAsync(request);
        var second = await service.CreateCustomerFromMobileAsync(request);

        Assert.True(first.Success);
        Assert.True(second.Success);
        Assert.Equal(first.Data!.CustomerId, second.Data!.CustomerId);
        Assert.Single(context.Customers.IgnoreQueryFilters());
    }

    [Fact]
    public async Task Title_WhenOcrTitleExists_UsesExistingTitle()
    {
        var title = SeedTitle("Genel Müdür");
        var (service, _) = await BuildServiceAsync(db => db.Titles.Add(title));

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest(title: "Genel Müdür"));

        Assert.True(result.Success);
        Assert.Equal(title.Id, result.Data!.TitleId);
        Assert.False(result.Data.UsedFallbackTitle);
        Assert.Equal("Reused", result.Data.TitleAction);
    }

    [Fact]
    public async Task Title_WhenOcrTitleMissingInSystem_UsesExistingUnknownTitle()
    {
        var unknownTitle = SeedTitle("Bilinmeyen");
        var (service, _) = await BuildServiceAsync(db => db.Titles.Add(unknownTitle));

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest(title: "Deputy General Manager"));

        Assert.True(result.Success);
        Assert.Equal(unknownTitle.Id, result.Data!.TitleId);
        Assert.True(result.Data.UsedFallbackTitle);
        Assert.Equal("Bilinmeyen", result.Data.ResolvedTitleName);
    }

    [Fact]
    public async Task Title_WhenOcrTitleMissingAndUnknownMissing_CreatesUnknownTitle()
    {
        var (service, context) = await BuildServiceAsync();

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest(title: "Deputy General Manager"));

        Assert.True(result.Success);
        Assert.True(result.Data!.TitleCreated);
        Assert.True(result.Data.UsedFallbackTitle);
        var unknownTitle = await context.Titles.IgnoreQueryFilters().FirstAsync();
        Assert.Equal("Bilinmeyen", unknownTitle.TitleName);
    }

    [Fact]
    public async Task Title_WhenOcrTitleNull_FallsBackToUnknownTitle()
    {
        var unknownTitle = SeedTitle("Bilinmeyen");
        var (service, _) = await BuildServiceAsync(db => db.Titles.Add(unknownTitle));

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest(title: null));

        Assert.True(result.Success);
        Assert.Equal(unknownTitle.Id, result.Data!.TitleId);
        Assert.True(result.Data.UsedFallbackTitle);
    }

    [Fact]
    public async Task Title_Normalization_IsCaseInsensitiveAndWhitespaceInsensitive()
    {
        var title = SeedTitle("GENEL MUDUR");
        var (service, _) = await BuildServiceAsync(db => db.Titles.Add(title));

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest(title: "  Genel   Müdür "));

        Assert.True(result.Success);
        Assert.Equal(title.Id, result.Data!.TitleId);
        Assert.False(result.Data.UsedFallbackTitle);
    }

    [Fact]
    public async Task Contact_WhenMissing_CreatesNewContact()
    {
        var existingCustomer = SeedCustomer("Windoform");
        var (service, context) = await BuildServiceAsync(db => db.Customers.Add(existingCustomer));

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest());

        Assert.True(result.Success);
        Assert.True(result.Data!.ContactCreated);
        Assert.Single(context.Contacts.IgnoreQueryFilters());
    }

    [Fact]
    public async Task Contact_WhenSameEmailExists_ReusesExistingContact()
    {
        var existingCustomer = SeedCustomer("Windoform");
        var existingTitle = SeedTitle("Bilinmeyen");
        var (service, context) = await BuildServiceAsync(db =>
        {
            db.Customers.Add(existingCustomer);
            db.Titles.Add(existingTitle);
            db.SaveChanges();
            db.Contacts.Add(SeedContact(existingCustomer.Id, email: "a.yuksel@windoform.com.tr", mobile: "05329998877", phone: "02320000000", titleId: existingTitle.Id));
        });

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest());

        Assert.True(result.Success);
        Assert.False(result.Data!.ContactCreated);
        Assert.Equal("Reused", result.Data.ContactAction);
        Assert.Single(context.Contacts.IgnoreQueryFilters());
    }

    [Fact]
    public async Task Contact_WhenSameNormalizedPhoneExists_ReusesExistingContact()
    {
        var existingCustomer = SeedCustomer("Windoform");
        var (service, context) = await BuildServiceAsync(db =>
        {
            db.Customers.Add(existingCustomer);
            db.SaveChanges();
            db.Contacts.Add(SeedContact(existingCustomer.Id, email: null, mobile: "+90 532 425 48 21", phone: null));
        });

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest(email: null, phone: "05324254821", phone2: null));

        Assert.True(result.Success);
        Assert.False(result.Data!.ContactCreated);
        Assert.Single(context.Contacts.IgnoreQueryFilters());
    }

    [Fact]
    public async Task Contact_WhenSameNameAndCompanyExists_ReusesExistingContact()
    {
        var existingCustomer = SeedCustomer("Windoform");
        var (service, context) = await BuildServiceAsync(db =>
        {
            db.Customers.Add(existingCustomer);
            db.SaveChanges();
            db.Contacts.Add(SeedContact(existingCustomer.Id, email: null, mobile: null, phone: null));
        });

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest(email: null, phone: null, phone2: null));

        Assert.True(result.Success);
        Assert.False(result.Data!.ContactCreated);
        Assert.Single(context.Contacts.IgnoreQueryFilters());
    }

    [Fact]
    public async Task Contact_WhenSoftDeleted_ReactivatesExistingContact()
    {
        var existingCustomer = SeedCustomer("Windoform");
        var (service, context) = await BuildServiceAsync(db =>
        {
            db.Customers.Add(existingCustomer);
            db.SaveChanges();
            db.Contacts.Add(SeedContact(existingCustomer.Id, isDeleted: true));
        });

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest());

        Assert.True(result.Success);
        Assert.Equal("Reactivated", result.Data!.ContactAction);
        var restored = await context.Contacts.IgnoreQueryFilters().FirstAsync();
        Assert.False(restored.IsDeleted);
    }

    [Fact]
    public async Task Contact_WhenTwoDifferentEmployeesSameCompany_CreatesTwoContactsUnderOneCompany()
    {
        var (service, context) = await BuildServiceAsync();

        var first = await service.CreateCustomerFromMobileAsync(BuildRequest(contactName: "Aydin Yuksel", email: "a.yuksel@windoform.com.tr", phone: "05324254821"));
        var second = await service.CreateCustomerFromMobileAsync(BuildRequest(contactName: "Volkan Saglik", email: "v.saglik@windoform.com.tr", phone: "05331580040"));

        Assert.True(first.Success);
        Assert.True(second.Success);
        Assert.Equal(first.Data!.CustomerId, second.Data!.CustomerId);
        Assert.Equal(1, await context.Customers.IgnoreQueryFilters().CountAsync());
        Assert.Equal(2, await context.Contacts.IgnoreQueryFilters().CountAsync());
    }

    [Fact]
    public async Task Contact_WhenSamePersonScannedTwice_DoesNotCreateDuplicateContact()
    {
        var (service, context) = await BuildServiceAsync();
        var request = BuildRequest();

        var first = await service.CreateCustomerFromMobileAsync(request);
        var second = await service.CreateCustomerFromMobileAsync(request);

        Assert.True(first.Success);
        Assert.True(second.Success);
        Assert.Equal(first.Data!.ContactId, second.Data!.ContactId);
        Assert.Single(context.Contacts.IgnoreQueryFilters());
    }

    [Fact]
    public async Task Integration_TwoEmployeesFromSameCompany_UsesSingleCompanyAndCorrectTitles()
    {
        var gmTitle = SeedTitle("Genel Müdür");
        var fallbackTitle = SeedTitle("Bilinmeyen");
        var (service, context) = await BuildServiceAsync(db =>
        {
            db.Titles.Add(gmTitle);
            db.Titles.Add(fallbackTitle);
        });

        var first = await service.CreateCustomerFromMobileAsync(BuildRequest(contactName: "Aydin Yuksel", email: "a.yuksel@windoform.com.tr", phone: "05324254821", title: "Genel Müdür"));
        var second = await service.CreateCustomerFromMobileAsync(BuildRequest(contactName: "Volkan Saglik", email: "v.saglik@windoform.com.tr", phone: "05331580040", title: "Deputy General Manager"));

        Assert.True(first.Success);
        Assert.True(second.Success);
        Assert.Equal(first.Data!.CustomerId, second.Data!.CustomerId);
        Assert.Equal(gmTitle.Id, first.Data.TitleId);
        Assert.Equal(fallbackTitle.Id, second.Data.TitleId);
        Assert.Equal(2, await context.Contacts.IgnoreQueryFilters().CountAsync());
    }

    [Fact]
    public async Task Integration_WhenOcrDataIsMissing_CompletesInControlledWay()
    {
        var (service, context) = await BuildServiceAsync();
        var request = BuildRequest(email: null, phone: null, phone2: null, contactName: "", title: null);

        var result = await service.CreateCustomerFromMobileAsync(request);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Null(result.Data!.ContactId);
        Assert.Equal(0, await context.Contacts.IgnoreQueryFilters().CountAsync());
    }

    [Fact]
    public async Task Integration_WhenEmailMissingAndPhoneExists_WorksCorrectly()
    {
        var (service, context) = await BuildServiceAsync();

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest(email: null, phone: "05324254821"));

        Assert.True(result.Success);
        Assert.Single(context.Contacts.IgnoreQueryFilters());
    }

    [Fact]
    public async Task Integration_WhenOnlyNameSurnameAndCompanyExist_WorksCorrectly()
    {
        var (service, context) = await BuildServiceAsync();

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest(email: null, phone: null, phone2: null, contactName: "Volkan Saglik"));

        Assert.True(result.Success);
        Assert.Single(context.Contacts.IgnoreQueryFilters());
    }

    [Fact]
    public async Task Integration_WhenSamePayloadCalledTwice_ReturnsIdempotentResult()
    {
        var (service, context) = await BuildServiceAsync();
        var request = BuildRequest();

        var first = await service.CreateCustomerFromMobileAsync(request);
        var second = await service.CreateCustomerFromMobileAsync(request);

        Assert.True(first.Success);
        Assert.True(second.Success);
        Assert.Equal(first.Data!.CustomerId, second.Data!.CustomerId);
        Assert.Equal(first.Data.ContactId, second.Data.ContactId);
        Assert.Equal(1, await context.Customers.IgnoreQueryFilters().CountAsync());
        Assert.Equal(1, await context.Contacts.IgnoreQueryFilters().CountAsync());
    }

    [Fact]
    public async Task Company_WhenMultipleDeletedDuplicatesExist_ReturnsConflict()
    {
        var (service, _) = await BuildServiceAsync(db =>
        {
            db.Customers.Add(SeedCustomer("Windoform", isDeleted: true));
            db.Customers.Add(SeedCustomer("Windoform", isDeleted: true));
        });

        var result = await service.CreateCustomerFromMobileAsync(BuildRequest());

        Assert.False(result.Success);
        Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
    }
}
