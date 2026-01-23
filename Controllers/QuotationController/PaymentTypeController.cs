using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentTypeController : ControllerBase
    {
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly ILocalizationService _localizationService;

        public PaymentTypeController(IPaymentTypeService paymentTypeService, ILocalizationService localizationService)
        {
            _paymentTypeService = paymentTypeService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _paymentTypeService.GetAllPaymentTypesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _paymentTypeService.GetPaymentTypeByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentTypeCreateDto createPaymentTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<PaymentTypeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("PaymentTypeController.InvalidModelState"),
                    _localizationService.GetLocalizedString("PaymentTypeController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _paymentTypeService.CreatePaymentTypeAsync(createPaymentTypeDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] PaymentTypeUpdateDto updatePaymentTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<PaymentTypeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("PaymentTypeController.InvalidModelState"),
                    _localizationService.GetLocalizedString("PaymentTypeController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _paymentTypeService.UpdatePaymentTypeAsync(id, updatePaymentTypeDto);            
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _paymentTypeService.DeletePaymentTypeAsync(id); 
            return StatusCode(result.StatusCode, result);
        }
    }
}
