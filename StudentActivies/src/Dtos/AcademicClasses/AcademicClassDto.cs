namespace StudentActivities.src.Dtos.AcademicClasses
{
    public class AcademicClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int FacultyId { get; set; }
        public string FacultyName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
    }
}