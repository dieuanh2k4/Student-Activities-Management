using StudentActivities.src.Dtos.AcademicClasses;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class AcademicClassMapper
    {
        public static AcademicClassDto ToDto(AcademicClasses academicClass)
        {
            return new AcademicClassDto
            {
                Id = academicClass.Id,
                Name = academicClass.Name ?? string.Empty,
                FacultyId = academicClass.FacultyId,
                FacultyName = academicClass.Faculties?.Name ?? string.Empty,
                TotalStudents = academicClass.Students?.Count ?? 0
            };
        }

        public static AcademicClasses ToEntity(CreateAcademicClassDto dto)
        {
            return new AcademicClasses
            {
                Name = dto.Name,
                FacultyId = dto.FacultyId
            };
        }

        public static void UpdateEntity(AcademicClasses academicClass, UpdateAcademicClassDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Name))
                academicClass.Name = dto.Name;

            if (dto.FacultyId.HasValue)
                academicClass.FacultyId = dto.FacultyId.Value;
        }
    }
}