using StudentActivities.src.Dtos.Notifications;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationDto ToDto(Notifications notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                Context = notification.Context ?? string.Empty,
                SendDate = notification.SendDate,
                EventId = notification.EventId,
                ClubId = notification.ClubId,
                StudentId = notification.StudentId,
                Status = notification.Status ?? "Chưa đọc",
                EventName = notification.Events?.Name,
                ClubName = notification.Clubs?.Name,
                StudentName = $"{notification.Students?.FirstName} {notification.Students?.LastName}".Trim()
            };
        }

        public static Notifications ToEntity(CreateNotificationDto dto, int studentId)
        {
            return new Notifications
            {
                Context = dto.Context,
                SendDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EventId = dto.EventId,
                ClubId = dto.ClubId,
                StudentId = studentId,
                Status = "Chưa đọc"
            };
        }
    }
}