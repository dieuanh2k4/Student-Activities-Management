namespace StudentActivities.src.Dtos.Notifications
{
    public class CreateNotificationDto
    {
        public string Context { get; set; } = string.Empty;
        public int? EventId { get; set; }
        public int? ClubId { get; set; }
        public List<int>? StudentIds { get; set; } // Để gửi cho nhiều sinh viên
        public bool SendToAllStudents { get; set; } = false; // Gửi cho tất cả sinh viên
        public bool SendToEventRegistered { get; set; } = false; // Gửi cho sinh viên đã đăng ký sự kiện
        public bool SendToClubMembers { get; set; } = false; // Gửi cho thành viên câu lạc bộ
    }
}