using crm_api.DTOs;
using crm_api.DTOs.NotificationDto;
using System.Threading.Tasks;

namespace crm_api.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResponse<PagedResponse<NotificationDto>>> GetUserNotificationsAsync( PagedRequest pagedRequest);
        Task<ApiResponse<int>> GetUnreadCountAsync(long userId);
        Task<ApiResponse<bool>> MarkAsReadAsync(long notificationId, long userId);
        Task<ApiResponse<bool>> MarkAllAsReadAsync(long userId);
        Task<ApiResponse<bool>> CreateNotificationAsync(CreateNotificationDto createNotificationDto);
        Task<ApiResponse<bool>> DeleteNotificationAsync(long notificationId, long userId);
    }
}
