namespace StudentActivities.src.Dtos.Organizers
{
    public class OrganizerDashboardDto
    {
        public List<ClubSummaryDto> Clubs { get; set; } = new();
        public List<EventSummaryDto> Events { get; set; } = new();
    }

    public class ClubSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Thumbnail { get; set; }
        public int MemberCount { get; set; }
    }

    public class EventSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public string Location { get; set; } = null!;
        public int CurrentRegistrations { get; set; }
        public int MaxCapacity { get; set; }
        public string Status { get; set; } = null!;
    }
}