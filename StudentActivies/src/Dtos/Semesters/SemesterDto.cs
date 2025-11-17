namespace StudentActivities.src.Dtos.Semesters
{
    public class SemesterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int DurationDays { get; set; }
    }
}