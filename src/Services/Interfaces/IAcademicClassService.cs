using StudentActivities.src.Dtos.AcademicClasses;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IAcademicClassService
    {
        Task<AcademicClassDto> CreateAsync(CreateAcademicClassDto dto);
        Task<List<AcademicClassDto>> GetAllAsync();
        Task<List<AcademicClassDto>> GetByFacultyIdAsync(int facultyId);
        Task<AcademicClassDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateAcademicClassDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}