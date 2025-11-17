namespace StudentActivities.src.Dtos.AcademicClasses
{
    public class CreateAcademicClassDto
    {
        public string Name { get; set; } = string.Empty;

        public int FacultyId { get; set; }
    }
}