namespace StudentActivities.src.Dtos.Semesters
{
    public class CreateSemesterDto
    {
        public string Name { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}