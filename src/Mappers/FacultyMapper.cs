using StudentActivities.src.Dtos.Faculties;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class FacultyMapper
    {
        public static FacultyDto ToDto(Faculties faculty)
        {
            return new FacultyDto
            {
                Id = faculty.Id,
                Name = faculty.Name ?? string.Empty,
                TotalAcademicClasses = faculty.AcademicClasses?.Count ?? 0,
                TotalStudents = faculty.AcademicClasses?.Sum(c => c.Students?.Count ?? 0) ?? 0
            };
        }

        public static Faculties ToEntity(CreateFacultyDto dto)
        {
            return new Faculties
            {
                Name = dto.Name
            };
        }

        public static void UpdateEntity(Faculties faculty, UpdateFacultyDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Name))
                faculty.Name = dto.Name;
        }
    }
}