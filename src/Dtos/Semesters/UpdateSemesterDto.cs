namespace StudentActivities.src.Dtos.Semesters
{
    public class UpdateSemesterDto
    {
        public string? Name { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
    }
}