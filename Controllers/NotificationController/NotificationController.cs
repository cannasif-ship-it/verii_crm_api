using crm_api.DTOs;
using crm_api.DTOs.NotificationDto;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace crm_api.Controllers.NotificationController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("user-notifications")]
        public async Task<IActionResult> GetUserNotifications([FromQuery] PagedRequest? pagedRequest)
        {
            // Default values if pagedRequest is null
            if (pagedRequest == null)
            {
                pagedRequest = new PagedRequest();
            }
            
            var result = await _notificationService.GetUserNotificationsAsync(pagedRequest);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _notificationService.GetUnreadCountAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _notificationService.MarkAsReadAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _notificationService.MarkAllAsReadAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(long id)
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _notificationService.DeleteNotificationAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
        
        // This endpoint might be used by admin or internal systems, or for testing
        // In a real scenario, notifications are usually triggered by system events, not directly via API
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto createDto)
        {
             // For security, you might want to restrict who can create notifications via API
             // For now, we assume the caller provides the UserId to send to.
             // If the current user is creating a notification for THEMSELVES, we can override:
             // createDto.UserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
             
             // But usually this endpoint is for "Send notification TO user X"
             
             var result = await _notificationService.CreateNotificationAsync(createDto);
             return StatusCode(result.StatusCode, result);
        }
    }
}
