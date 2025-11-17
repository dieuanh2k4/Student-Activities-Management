namespace StudentActivities.src.Dtos.TrainingScores
{
    public class BulkUpdateResultDto
    {
        public int TotalStudents { get; set; }
        public int UpdatedCount { get; set; }
        public int CreatedCount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
