namespace StudentActivities.src.Dtos.Faculties
{
    public class FacultyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalAcademicClasses { get; set; }
        public int TotalStudents { get; set; }
    }
}