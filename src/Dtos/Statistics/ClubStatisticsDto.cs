namespace StudentActivities.src.Dtos.Statistics
{
    public class ClubStatisticsDto
    {
        public int TotalClubs { get; set; }
        public List<ClubByDateDto> ByDate { get; set; } = new();
        public string Period { get; set; } = string.Empty;
    }

    public class ClubByDateDto
    {
        public DateTime Date { get; set; }
        public string DateLabel { get; set; } = string.Empty;
        public int ClubCount { get; set; }
        public int TotalMembers { get; set; }
        public List<ClubSummaryDto> Clubs { get; set; } = new();
    }

    public class ClubSummaryDto
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int MemberCount { get; set; }
        public int EventCount { get; set; }
    }

    public class ClubStatisticsFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Period { get; set; } = "monthly";
    }
}