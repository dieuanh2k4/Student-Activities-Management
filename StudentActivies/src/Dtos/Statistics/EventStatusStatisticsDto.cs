namespace StudentActivities.src.Dtos.Statistics
{
    public class EventStatusStatisticsDto
    {
        public int TotalEvents { get; set; }
        public List<EventByStatusDto> ByStatus { get; set; } = new();
        public Dictionary<string, int> StatusCounts { get; set; } = new();
    }

    public class EventByStatusDto
    {
        public string Status { get; set; } = string.Empty; // "Upcoming", "Ongoing", "Completed", "Cancelled"
        public int Count { get; set; }
        public double Percentage { get; set; }
        public List<EventDetailDto> Events { get; set; } = new();
    }

    public class EventDetailDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public int RegisteredCount { get; set; }
        public int MaxParticipants { get; set; }
        public string OrganizerName { get; set; } = string.Empty;
    }

    public class EventStatusFilterDto
    {
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? OrganizerId { get; set; }
    }
}