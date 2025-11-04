namespace StudentActivities.src.Dtos.Notifications
{
    public class NotificationFilterDto
    {
        public int? StudentId { get; set; }
        public int? EventId { get; set; }
        public int? ClubId { get; set; }
        public string? Status { get; set; } // "Đã đọc", "Chưa đọc", null = tất cả
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "SendDate"; // SendDate, Status
        public string SortOrder { get; set; } = "desc"; // asc, desc
    }
}