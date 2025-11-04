namespace StudentActivities.src.Dtos.Notifications
{
    public class NotificationSummaryDto
    {
        public int TotalNotifications { get; set; }
        public int UnreadNotifications { get; set; }
        public int ReadNotifications { get; set; }
        public List<NotificationDto> RecentNotifications { get; set; } = new();
    }
}