namespace StudentActivities.src.Dtos.Reports
{
    public class ScoreByPeriodFilterDto
    {
        public int? SemesterId { get; set; }
        public int? ClassId { get; set; }
        public int? FacultyId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
