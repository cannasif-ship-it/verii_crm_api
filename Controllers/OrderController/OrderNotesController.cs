using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderNotesController : ControllerBase
    {
        private readonly IOrderNotesService _orderNotesService;

        public OrderNotesController(IOrderNotesService orderNotesService)
        {
            _orderNotesService = orderNotesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderNotes([FromQuery] PagedRequest request)
        {
            var result = await _orderNotesService.GetAllOrderNotesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderNotesById(long id)
        {
            var result = await _orderNotesService.GetOrderNotesByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(long orderId)
        {
            var result = await _orderNotesService.GetNotesByOrderIdAsync(orderId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderNotes([FromBody] CreateOrderNotesDto request)
        {
            var result = await _orderNotesService.CreateOrderNotesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderNotes(long id, [FromBody] UpdateOrderNotesDto request)
        {
            var result = await _orderNotesService.UpdateOrderNotesAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("by-order/{orderId}/notes-list")]
        public async Task<IActionResult> UpdateNotesListByOrderId(long orderId, [FromBody] UpdateOrderNotesListDto request)
        {
            var result = await _orderNotesService.UpdateNotesListByOrderIdAsync(orderId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderNotes(long id)
        {
            var result = await _orderNotesService.DeleteOrderNotesAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
