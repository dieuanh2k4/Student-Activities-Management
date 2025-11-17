using StudentActivities.src.Dtos.Faculties;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IFacultyService
    {
        Task<FacultyDto> CreateAsync(CreateFacultyDto dto);
        Task<List<FacultyDto>> GetAllAsync();
        Task<FacultyDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateFacultyDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}