using Microsoft.AspNetCore.Mvc;
using crm_api.DTOs;
using Microsoft.AspNetCore.Authorization;
using crm_api.Interfaces;

namespace crm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuotationController : ControllerBase
    {
        private readonly IQuotationService _quotationService;
        private readonly ILocalizationService _localizationService;

        public QuotationController(IQuotationService quotationService, ILocalizationService localizationService)
        {
            _quotationService = quotationService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm teklifleri getirir
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet]
        public async Task<IActionResult> GetQuotations([FromQuery] PagedRequest request)
        {
            var result = await _quotationService.GetAllQuotationsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre teklif getirir
        /// </summary>
        /// <param name="id">Teklif ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuotation(long id)
        {
            var result = await _quotationService.GetQuotationByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni teklif oluşturur
        /// </summary>
        /// <param name="createQuotationDto">Teklif oluşturma bilgileri</param>
        /// <returns>ApiResponse</returns>
        [HttpPost]
        public async Task<IActionResult> CreateQuotation([FromBody] CreateQuotationDto createQuotationDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<QuotationDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationController.InvalidModelState"),
                    _localizationService.GetLocalizedString("QuotationController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _quotationService.CreateQuotationAsync(createQuotationDto);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("bulk-quotation")]
        public async Task<IActionResult> CreateQuotationBulk([FromBody] QuotationBulkCreateDto bulkDto)
        {

            var result = await _quotationService.CreateQuotationBulkAsync(bulkDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("revision-of-quotation")]
        public async Task<IActionResult> CreateRevisionOfQuotation([FromBody] long quotationId)
        {
            var result = await _quotationService.CreateRevisionOfQuotationAsync(quotationId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("price-rule-of-quotation")]
        public async Task<IActionResult> GetPriceRuleOfQuotation([FromQuery] string customerCode,[FromQuery] long salesmenId,[FromQuery] DateTime quotationDate)
        {
            var result = await _quotationService.GetPriceRuleOfQuotationAsync(customerCode,salesmenId,quotationDate);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("price-of-product")]
        public async Task<IActionResult> GetPriceOfProduct([FromQuery] List<PriceOfProductRequestDto> request)
        {
            var result = await _quotationService.GetPriceOfProductAsync(request);
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
                    _localizationService.GetLocalizedString("QuotationController.InvalidModelState"),
                    _localizationService.GetLocalizedString("QuotationController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _quotationService.StartApprovalFlowAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Kullanıcının bekleyen onaylarını getirir
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("waiting-approvals")]
        public async Task<IActionResult> GetWaitingApprovals()
        {
            var result = await _quotationService.GetWaitingApprovalsAsync();
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
                    _localizationService.GetLocalizedString("QuotationController.InvalidModelState"),
                    _localizationService.GetLocalizedString("QuotationController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _quotationService.ApproveAsync(request);
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
                    _localizationService.GetLocalizedString("QuotationController.InvalidModelState"),
                    _localizationService.GetLocalizedString("QuotationController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _quotationService.RejectAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Teklifi günceller
        /// </summary>
        /// <param name="id">Teklif ID</param>
        /// <param name="updateQuotationDto">Güncellenecek teklif bilgileri</param>
        /// <returns>ApiResponse</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuotation(long id, [FromBody] UpdateQuotationDto updateQuotationDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<QuotationDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationController.InvalidModelState"),
                    _localizationService.GetLocalizedString("QuotationController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _quotationService.UpdateQuotationAsync(id, updateQuotationDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Teklifi siler
        /// </summary>
        /// <param name="id">Teklif ID</param>
        /// <returns>ApiResponse</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuotation(long id)
        {
            var result = await _quotationService.DeleteQuotationAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Potansiyel müşteriye göre teklifleri getirir
        /// </summary>
        /// <param name="potentialCustomerId">Potansiyel müşteri ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("by-potential-customer/{potentialCustomerId}")]
        public async Task<IActionResult> GetQuotationsByPotentialCustomer(long potentialCustomerId)
        {
            var result = await _quotationService.GetQuotationsByPotentialCustomerIdAsync(potentialCustomerId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Temsilciye göre teklifleri getirir
        /// </summary>
        /// <param name="representativeId">Temsilci ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("by-representative/{representativeId}")]
        public async Task<IActionResult> GetQuotationsByRepresentative(long representativeId)
        {
            var result = await _quotationService.GetQuotationsByRepresentativeIdAsync(representativeId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Duruma göre teklifleri getirir
        /// </summary>
        /// <param name="status">Durum</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("by-status/{status}")]
        public async Task<IActionResult> GetQuotationsByStatus(int status)
        {
            var result = await _quotationService.GetQuotationsByStatusAsync(status);
            return StatusCode(result.StatusCode, result);
        }
    }
}
