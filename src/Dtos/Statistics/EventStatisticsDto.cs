namespace StudentActivities.src.Dtos.Statistics
{
    public class EventStatisticsDto
    {
        public int TotalEvents { get; set; }
        public List<EventByDateDto> ByDate { get; set; } = new();
        public string Period { get; set; } = string.Empty; // "daily", "monthly", "yearly"
    }

    public class EventByDateDto
    {
        public DateTime Date { get; set; }
        public string DateLabel { get; set; } = string.Empty; // "2024-11-09", "2024-11", "2024"
        public int EventCount { get; set; }
        public int TotalParticipants { get; set; }
        public List<EventSummaryDto> Events { get; set; } = new();
    }

    public class EventSummaryDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ParticipantCount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class EventStatisticsFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Period { get; set; } = "daily"; // "daily", "monthly", "yearly"
    }
}