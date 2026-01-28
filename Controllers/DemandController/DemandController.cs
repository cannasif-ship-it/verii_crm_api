using Microsoft.AspNetCore.Mvc;
using crm_api.DTOs;
using Microsoft.AspNetCore.Authorization;
using crm_api.Interfaces;

namespace crm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DemandController : ControllerBase
    {
        private readonly IDemandService _demandService;
        private readonly ILocalizationService _localizationService;

        public DemandController(IDemandService demandService, ILocalizationService localizationService)
        {
            _demandService = demandService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm talepleri getirir
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet]
        public async Task<IActionResult> GetDemands([FromQuery] PagedRequest request)
        {
            var result = await _demandService.GetAllDemandsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Kullanıcıya göre talepleri getirir
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("related")]
        public async Task<IActionResult> GetRelatedDemands([FromQuery] PagedRequest request)
        {
            var result = await _demandService.GetRelatedDemands(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre talep getirir
        /// </summary>
        /// <param name="id">Talep ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDemand(long id)
        {
            var result = await _demandService.GetDemandByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni talep oluşturur
        /// </summary>
        /// <param name="createDemandDto">Talep oluşturma bilgileri</param>
        /// <returns>ApiResponse</returns>
        [HttpPost]
        public async Task<IActionResult> CreateDemand([FromBody] CreateDemandDto createDemandDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<DemandDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandController.InvalidModelState"),
                    _localizationService.GetLocalizedString("DemandController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _demandService.CreateDemandAsync(createDemandDto);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("bulk-demand")]
        public async Task<IActionResult> CreateDemandBulk([FromBody] DemandBulkCreateDto bulkDto)
        {

            var result = await _demandService.CreateDemandBulkAsync(bulkDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("revision-of-demand")]
        public async Task<IActionResult> CreateRevisionOfDemand([FromBody] long demandId)
        {
            var result = await _demandService.CreateRevisionOfDemandAsync(demandId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("price-rule-of-demand")]
        public async Task<IActionResult> GetPriceRuleOfDemand([FromQuery] string customerCode,[FromQuery] long salesmenId,[FromQuery] DateTime demandDate)
        {
            var result = await _demandService.GetPriceRuleOfDemandAsync(customerCode,salesmenId,demandDate);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("price-of-product")]
        public async Task<IActionResult> GetPriceOfProduct([FromQuery] List<PriceOfProductRequestDto> request)
        {
            var result = await _demandService.GetPriceOfProductAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Onay akışını başlatır
        /// </summary>
        /// <param name="request">Onay akışı başlatma bilgileri</param>
        /// <returns>ApiResponse</returns>
        [HttpPost("start-approval-flow")]
        public async Task<IActionResult> StartApprovalFlow([FromBody] StartApprovalFlowDto request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandController.InvalidModelState"),
                    _localizationService.GetLocalizedString("DemandController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _demandService.StartApprovalFlowAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Kullanıcının bekleyen onaylarını getirir
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("waiting-approvals")]
        public async Task<IActionResult> GetWaitingApprovals()
        {
            var result = await _demandService.GetWaitingApprovalsAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Onay işlemini gerçekleştirir
        /// </summary>
        /// <param name="request">Onay işlemi bilgileri</param>
        /// <returns>ApiResponse</returns>
        [HttpPost("approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveActionDto request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandController.InvalidModelState"),
                    _localizationService.GetLocalizedString("DemandController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _demandService.ApproveAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Red işlemini gerçekleştirir
        /// </summary>
        /// <param name="request">Red işlemi bilgileri</param>
        /// <returns>ApiResponse</returns>
        [HttpPost("reject")]
        public async Task<IActionResult> Reject([FromBody] RejectActionDto request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandController.InvalidModelState"),
                    _localizationService.GetLocalizedString("DemandController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _demandService.RejectAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Talebi günceller
        /// </summary>
        /// <param name="id">Talep ID</param>
        /// <param name="updateDemandDto">Güncellenecek talep bilgileri</param>
        /// <returns>ApiResponse</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDemand(long id, [FromBody] UpdateDemandDto updateDemandDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<DemandDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandController.InvalidModelState"),
                    _localizationService.GetLocalizedString("DemandController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _demandService.UpdateDemandAsync(id, updateDemandDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Talebi siler
        /// </summary>
        /// <param name="id">Talep ID</param>
        /// <returns>ApiResponse</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDemand(long id)
        {
            var result = await _demandService.DeleteDemandAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Potansiyel müşteriye göre talepleri getirir
        /// </summary>
        /// <param name="potentialCustomerId">Potansiyel müşteri ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("by-potential-customer/{potentialCustomerId}")]
        public async Task<IActionResult> GetDemandsByPotentialCustomer(long potentialCustomerId)
        {
            var result = await _demandService.GetDemandsByPotentialCustomerIdAsync(potentialCustomerId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Temsilciye göre talepleri getirir
        /// </summary>
        /// <param name="representativeId">Temsilci ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("by-representative/{representativeId}")]
        public async Task<IActionResult> GetDemandsByRepresentative(long representativeId)
        {
            var result = await _demandService.GetDemandsByRepresentativeIdAsync(representativeId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Duruma göre talepleri getirir
        /// </summary>
        /// <param name="status">Durum</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("by-status/{status}")]
        public async Task<IActionResult> GetDemandsByStatus(int status)
        {
            var result = await _demandService.GetDemandsByStatusAsync(status);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Kullanıcıya göre talep ile ilgili kullanıcıları getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("related-users/{userId}")]
        public async Task<IActionResult> GetDemandRelatedUsers(long userId)
        {
            var result = await _demandService.GetDemandRelatedUsersAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
