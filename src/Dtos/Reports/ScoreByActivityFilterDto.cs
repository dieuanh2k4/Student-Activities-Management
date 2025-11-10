namespace StudentActivities.src.Dtos.Reports
{
    public class ScoreByActivityFilterDto
    {
        public int StudentId { get; set; }
        public int? EventId { get; set; }
        public int? ClubId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
