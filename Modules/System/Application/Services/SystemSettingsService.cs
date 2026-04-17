using AutoMapper;
using crm_api.Modules.System.Application.Dtos;
using crm_api.Shared.Common.Application;
using crm_api.Shared.Infrastructure.Abstractions;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Modules.System.Application.Services
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SystemSettingsService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<SystemSettingsDto>> GetAsync()
        {
            try
            {
                var entity = await _unitOfWork.SystemSettings
                    .Query()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (entity == null)
                {
                    return ApiResponse<SystemSettingsDto>.SuccessResult(
                        new SystemSettingsDto(),
                        _localizationService.GetLocalizedString("SystemSettingsService.DefaultSystemSettingsReturned"));
                }

                return ApiResponse<SystemSettingsDto>.SuccessResult(
                    _mapper.Map<SystemSettingsDto>(entity),
                    _localizationService.GetLocalizedString("SystemSettingsService.SystemSettingsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SystemSettingsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SystemSettingsService.InternalServerError"),
                    _localizationService.GetLocalizedString("SystemSettingsService.GetExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SystemSettingsDto>> UpdateAsync(UpdateSystemSettingsDto dto, long userId)
        {
            try
            {
                var entity = await _unitOfWork.SystemSettings
                    .Query()
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (entity == null)
                {
                    entity = new Domain.Entities.SystemSetting
                    {
                        IsDeleted = false,
                        CreatedDate = DateTimeProvider.Now,
                        CreatedBy = userId
                    };

                    _mapper.Map(dto, entity);
                    entity.UpdatedDate = DateTimeProvider.Now;
                    entity.UpdatedBy = userId;

                    await _unitOfWork.SystemSettings.AddAsync(entity).ConfigureAwait(false);
                    await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                    return ApiResponse<SystemSettingsDto>.SuccessResult(
                        _mapper.Map<SystemSettingsDto>(entity),
                        _localizationService.GetLocalizedString("SystemSettingsService.SystemSettingsCreated"));
                }

                _mapper.Map(dto, entity);
                entity.UpdatedDate = DateTimeProvider.Now;
                entity.UpdatedBy = userId;

                await _unitOfWork.SystemSettings.UpdateAsync(entity).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<SystemSettingsDto>.SuccessResult(
                    _mapper.Map<SystemSettingsDto>(entity),
                    _localizationService.GetLocalizedString("SystemSettingsService.SystemSettingsUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SystemSettingsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("SystemSettingsService.InternalServerError"),
                    _localizationService.GetLocalizedString("SystemSettingsService.UpdateExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
