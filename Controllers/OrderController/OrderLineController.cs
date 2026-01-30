using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using crm_api.DTOs;
using crm_api.Interfaces;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderLineController : ControllerBase
    {
        private readonly IOrderLineService _orderLineService;
        private readonly ILocalizationService _localizationService;

        public OrderLineController(IOrderLineService orderLineService, ILocalizationService localizationService)
        {
            _orderLineService = orderLineService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm sipariş satırlarını getirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOrderLines([FromQuery] PagedRequest request)
        {
            var result = await _orderLineService.GetAllOrderLinesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID’ye göre sipariş satırını getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderLine(long id)
        {
            var result = await _orderLineService.GetOrderLineByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni sipariş satırı oluşturur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateOrderLine([FromBody] CreateOrderLineDto createOrderLineDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<OrderLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderLineController.InvalidModelState"),
                    _localizationService.GetLocalizedString("OrderLineController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _orderLineService.CreateOrderLineAsync(createOrderLineDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Birden fazla sipariş satırı oluşturur
        /// </summary>
        [HttpPost("create-multiple")]
        public async Task<IActionResult> CreateOrderLines([FromBody] List<CreateOrderLineDto> createOrderLineDtos)
        {
            var result = await _orderLineService.CreateOrderLinesAsync(createOrderLineDtos);
            return StatusCode(result.StatusCode, result);
        }
        
        /// <summary>
        /// Birden fazla sipariş satırını günceller
        /// </summary>
        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateOrderLines([FromBody] List<OrderLineDto> orderLineDtos)
        {
            var result = await _orderLineService.UpdateOrderLinesAsync(orderLineDtos);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Sipariş satırını günceller
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderLine(long id, [FromBody] UpdateOrderLineDto updateOrderLineDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<OrderLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderLineController.InvalidModelState"),
                    _localizationService.GetLocalizedString("OrderLineController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _orderLineService.UpdateOrderLineAsync(id, updateOrderLineDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Sipariş satırını siler
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderLine(long id)
        {
            var result = await _orderLineService.DeleteOrderLineAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Siparişe göre sipariş satırlarını getirir
        /// </summary>
        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetOrderLinesByOrderId(long orderId)
        {
            var result = await _orderLineService.GetOrderLinesByOrderIdAsync(orderId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
