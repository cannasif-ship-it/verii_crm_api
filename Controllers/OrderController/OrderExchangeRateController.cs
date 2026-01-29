using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderExchangeRateController : ControllerBase
    {
        private readonly IOrderExchangeRateService _orderExchangeRateService;

        public OrderExchangeRateController(IOrderExchangeRateService orderExchangeRateService)
        {
            _orderExchangeRateService = orderExchangeRateService;
        }

        /// <summary>
        /// Tüm sipariş döviz kurlarını getirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _orderExchangeRateService.GetAllOrderExchangeRatesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre sipariş döviz kurunu getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _orderExchangeRateService.GetOrderExchangeRateByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Siparişe göre sipariş döviz kurlarını getirir
        /// </summary>
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(long orderId)
        {
            var result = await _orderExchangeRateService.GetOrderExchangeRatesByOrderIdAsync(orderId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni sipariş döviz kuru oluşturur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderExchangeRateCreateDto createDto)
        {
            var result = await _orderExchangeRateService.CreateOrderExchangeRateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Sipariş döviz kurunu günceller
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] OrderExchangeRateUpdateDto updateDto)
        {
            var result = await _orderExchangeRateService.UpdateOrderExchangeRateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Sipariş içindeki döviz kurlarını toplu günceller
        /// </summary>
        [HttpPut("update-exchange-rate-in-order")]
        public async Task<IActionResult> UpdateExchangeRateInOrder([FromBody] List<OrderExchangeRateGetDto> updateDtos)
        {
            var result = await _orderExchangeRateService.UpdateExchangeRateInOrder(updateDtos);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Sipariş döviz kurunu siler
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _orderExchangeRateService.DeleteOrderExchangeRateAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
