using System;

namespace StudentActivities.src.Dtos.TrainingScores
{
    public class TrainingScoreDto
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateOnly DateAssigned { get; set; }
        public int EventId { get; set; }
        public int StudentId { get; set; }
        public int SemesterId { get; set; }

        // Thông tin bổ sung
        public string? EventName { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? SemesterName { get; set; }
        public string? FacultyName { get; set; }
        public string? ClassName { get; set; }
    }
}
