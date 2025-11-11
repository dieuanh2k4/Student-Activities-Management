namespace StudentActivities.src.Dtos.TrainingScores
{
    public class TrainingScoreFilterDto
    {
        public int? StudentId { get; set; }
        public int? EventId { get; set; }
        public int? SemesterId { get; set; }
        public int? FacultyId { get; set; }
        public int? ClassId { get; set; }
        public string? StudentCode { get; set; }
        public string? StudentName { get; set; }

        // Pagination
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
