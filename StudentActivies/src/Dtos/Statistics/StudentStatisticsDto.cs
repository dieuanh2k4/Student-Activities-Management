namespace StudentActivities.src.Dtos.Statistics
{
    public class StudentStatisticsDto
    {
        public int TotalStudents { get; set; }
        public List<StudentByClassDto> ByClass { get; set; } = new();
        public List<StudentByFacultyDto> ByFaculty { get; set; } = new();
        public List<StudentBySemesterDto> BySemester { get; set; } = new();
    }

    public class StudentByClassDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public double Percentage { get; set; }
    }

    public class StudentByFacultyDto
    {
        public int FacultyId { get; set; }
        public string FacultyName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public double Percentage { get; set; }
    }

    public class StudentBySemesterDto
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public double Percentage { get; set; }
    }

    public class StudentStatisticsFilterDto
    {
        public int? ClassId { get; set; }
        public int? FacultyId { get; set; }
        public int? SemesterId { get; set; }
    }
}