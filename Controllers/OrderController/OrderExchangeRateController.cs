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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _orderExchangeRateService.GetAllOrderExchangeRatesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _orderExchangeRateService.GetOrderExchangeRateByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(long orderId)
        {
            var result = await _orderExchangeRateService.GetOrderExchangeRatesByOrderIdAsync(orderId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderExchangeRateCreateDto createDto)
        {
            var result = await _orderExchangeRateService.CreateOrderExchangeRateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] OrderExchangeRateUpdateDto updateDto)
        {
            var result = await _orderExchangeRateService.UpdateOrderExchangeRateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _orderExchangeRateService.DeleteOrderExchangeRateAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
