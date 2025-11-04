namespace StudentActivities.src.Dtos.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Context { get; set; } = string.Empty;
        public DateOnly SendDate { get; set; }
        public int? EventId { get; set; }
        public int? ClubId { get; set; }
        public int StudentId { get; set; }
        public string Status { get; set; } = "Chưa đọc";

        public string? EventName { get; set; }
        public string? ClubName { get; set; }
        public string? StudentName { get; set; }
    }
}