using Microsoft.AspNetCore.Mvc;
using crm_api.DTOs;
using Microsoft.AspNetCore.Authorization;
using crm_api.Interfaces;

namespace crm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILocalizationService _localizationService;

        public OrderController(IOrderService orderService, ILocalizationService localizationService)
        {
            _orderService = orderService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm siparişleri getirir
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] PagedRequest request)
        {
            var result = await _orderService.GetAllOrdersAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Kullanıcıya göre siparişleri getirir
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("related")]
        public async Task<IActionResult> GetRelatedOrders([FromQuery] PagedRequest request)
        {
            var result = await _orderService.GetRelatedOrders(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre sipariş getirir
        /// </summary>
        /// <param name="id">Sipariş ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(long id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni sipariş oluşturur
        /// </summary>
        /// <param name="createOrderDto">Sipariş oluşturma bilgileri</param>
        /// <returns>ApiResponse</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<OrderDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderController.InvalidModelState"),
                    _localizationService.GetLocalizedString("OrderController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _orderService.CreateOrderAsync(createOrderDto);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("bulk-order")]
        public async Task<IActionResult> CreateOrderBulk([FromBody] OrderBulkCreateDto bulkDto)
        {

            var result = await _orderService.CreateOrderBulkAsync(bulkDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("revision-of-order")]
        public async Task<IActionResult> CreateRevisionOfOrder([FromBody] long orderId)
        {
            var result = await _orderService.CreateRevisionOfOrderAsync(orderId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("price-rule-of-order")]
        public async Task<IActionResult> GetPriceRuleOfOrder([FromQuery] string customerCode,[FromQuery] long salesmenId,[FromQuery] DateTime orderDate)
        {
            var result = await _orderService.GetPriceRuleOfOrderAsync(customerCode,salesmenId,orderDate);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("price-of-product")]
        public async Task<IActionResult> GetPriceOfProduct([FromQuery] List<PriceOfProductRequestDto> request)
        {
            var result = await _orderService.GetPriceOfProductAsync(request);
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
                    _localizationService.GetLocalizedString("OrderController.InvalidModelState"),
                    _localizationService.GetLocalizedString("OrderController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _orderService.StartApprovalFlowAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Kullanıcının bekleyen onaylarını getirir
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("waiting-approvals")]
        public async Task<IActionResult> GetWaitingApprovals()
        {
            var result = await _orderService.GetWaitingApprovalsAsync();
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
                    _localizationService.GetLocalizedString("OrderController.InvalidModelState"),
                    _localizationService.GetLocalizedString("OrderController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _orderService.ApproveAsync(request);
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
                    _localizationService.GetLocalizedString("OrderController.InvalidModelState"),
                    _localizationService.GetLocalizedString("OrderController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _orderService.RejectAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Siparişi günceller
        /// </summary>
        /// <param name="id">Sipariş ID</param>
        /// <param name="updateOrderDto">Güncellenecek sipariş bilgileri</param>
        /// <returns>ApiResponse</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(long id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<OrderDto>.ErrorResult(
                    _localizationService.GetLocalizedString("OrderController.InvalidModelState"),
                    _localizationService.GetLocalizedString("OrderController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _orderService.UpdateOrderAsync(id, updateOrderDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Siparişi siler
        /// </summary>
        /// <param name="id">Sipariş ID</param>
        /// <returns>ApiResponse</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Potansiyel müşteriye göre siparişleri getirir
        /// </summary>
        /// <param name="potentialCustomerId">Potansiyel müşteri ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("by-potential-customer/{potentialCustomerId}")]
        public async Task<IActionResult> GetOrdersByPotentialCustomer(long potentialCustomerId)
        {
            var result = await _orderService.GetOrdersByPotentialCustomerIdAsync(potentialCustomerId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Temsilciye göre siparişleri getirir
        /// </summary>
        /// <param name="representativeId">Temsilci ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("by-representative/{representativeId}")]
        public async Task<IActionResult> GetOrdersByRepresentative(long representativeId)
        {
            var result = await _orderService.GetOrdersByRepresentativeIdAsync(representativeId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Duruma göre siparişleri getirir
        /// </summary>
        /// <param name="status">Durum</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("by-status/{status}")]
        public async Task<IActionResult> GetOrdersByStatus(int status)
        {
            var result = await _orderService.GetOrdersByStatusAsync(status);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Kullanıcıya göre sipariş ile ilgili kullanıcıları getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>ApiResponse</returns>
        [HttpGet("related-users/{userId}")]
        public async Task<IActionResult> GetOrderRelatedUsers(long userId)
        {
            var result = await _orderService.GetOrderRelatedUsersAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
