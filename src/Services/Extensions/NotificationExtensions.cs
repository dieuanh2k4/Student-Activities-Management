using StudentActivities.src.Dtos.Notifications;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Extensions
{
    public static class NotificationExtensions
    {
        /// <summary>
        /// Gửi thông báo tự động khi có sự kiện mới
        /// </summary>
        public static async Task SendEventNotificationAsync(
            this INotificationService notificationService,
            int eventId,
            string eventName,
            NotificationType type = NotificationType.EventCreated)
        {
            var context = type switch
            {
                NotificationType.EventCreated => $"🎉 Sự kiện mới '{eventName}' đã được tạo! Hãy đăng ký tham gia ngay.",
                NotificationType.EventUpdated => $"📝 Sự kiện '{eventName}' đã được cập nhật thông tin. Vui lòng kiểm tra lại.",
                NotificationType.EventReminder => $"⏰ Nhắc nhở: Sự kiện '{eventName}' sẽ diễn ra sớm. Đừng quên tham gia!",
                NotificationType.EventCancelled => $"❌ Sự kiện '{eventName}' đã bị hủy. Xin lỗi vì sự bất tiện này.",
                _ => $"📢 Thông báo về sự kiện '{eventName}'"
            };

            var dto = new CreateNotificationDto
            {
                Context = context,
                EventId = eventId,
                SendToAllStudents = type == NotificationType.EventCreated,
                SendToEventRegistered = type != NotificationType.EventCreated
            };

            await notificationService.CreateNotificationAsync(dto);
        }

        /// <summary>
        /// Gửi thông báo tự động khi có hoạt động câu lạc bộ
        /// </summary>
        public static async Task SendClubNotificationAsync(
            this INotificationService notificationService,
            int clubId,
            string clubName,
            string message,
            bool sendToAllStudents = false)
        {
            var context = $"🏛️ Thông báo từ CLB {clubName}: {message}";

            var dto = new CreateNotificationDto
            {
                Context = context,
                ClubId = clubId,
                SendToAllStudents = sendToAllStudents,
                SendToClubMembers = !sendToAllStudents
            };

            await notificationService.CreateNotificationAsync(dto);
        }

        /// <summary>
        /// Gửi thông báo nhắc nhở điểm rèn luyện
        /// </summary>
        public static async Task SendTrainingScoreNotificationAsync(
            this INotificationService notificationService,
            int studentId,
            int score,
            string eventName)
        {
            var context = $"📊 Bạn đã nhận được {score} điểm rèn luyện từ sự kiện '{eventName}'. Tổng điểm hiện tại của bạn đã được cập nhật.";

            var dto = new CreateNotificationDto
            {
                Context = context,
                StudentIds = new List<int> { studentId }
            };

            await notificationService.CreateNotificationAsync(dto);
        }

        /// <summary>
        /// Gửi thông báo hệ thống
        /// </summary>
        public static async Task SendSystemNotificationAsync(
            this INotificationService notificationService,
            string message,
            List<int>? studentIds = null,
            bool sendToAll = true)
        {
            var context = $"🔔 Thông báo hệ thống: {message}";

            var dto = new CreateNotificationDto
            {
                Context = context,
                SendToAllStudents = sendToAll && (studentIds == null || !studentIds.Any()),
                StudentIds = studentIds
            };

            await notificationService.CreateNotificationAsync(dto);
        }
    }

    public enum NotificationType
    {
        EventCreated,
        EventUpdated,
        EventReminder,
        EventCancelled,
        ClubActivity,
        TrainingScore,
        SystemMessage
    }
}