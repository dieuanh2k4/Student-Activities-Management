using StudentActivities.src.Dtos.Notifications;

namespace StudentActivities.src.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> CreateNotificationAsync(CreateNotificationDto dto);
        Task<List<NotificationDto>> GetNotificationsByStudentIdAsync(int studentId);
        Task<List<NotificationDto>> GetAllNotificationsAsync();
        Task<NotificationDto?> GetNotificationByIdAsync(int id);
        Task<bool> UpdateNotificationStatusAsync(int id, UpdateNotificationStatusDto dto);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> MarkAsUnreadAsync(int id);
        Task<NotificationSummaryDto> GetNotificationSummaryByStudentIdAsync(int studentId);
        Task<List<NotificationDto>> GetNotificationsByEventIdAsync(int eventId);
        Task<List<NotificationDto>> GetNotificationsByClubIdAsync(int clubId);
        Task<bool> DeleteNotificationAsync(int id);
        Task<int> GetUnreadCountByStudentIdAsync(int studentId);
        Task<PagedNotificationDto> GetNotificationsWithFilterAsync(NotificationFilterDto filter);
        Task<bool> MarkAllAsReadAsync(int studentId);
        Task<bool> DeleteAllNotificationsAsync(int studentId);
    }
}