namespace StudentActivities.src.Dtos.Reports
{
    public class TrainingScoreReportDto
    {
        public int StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string FacultyName { get; set; } = string.Empty;
        public int TotalScore { get; set; }
        public int EventCount { get; set; }
        public List<ActivityDetailDto> Activities { get; set; } = new();
    }

    public class ActivityDetailDto
    {
        public string ActivityName { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}
