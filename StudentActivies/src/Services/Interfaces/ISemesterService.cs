using StudentActivities.src.Dtos.Semesters;

namespace StudentActivities.src.Services.Interfaces
{
    public interface ISemesterService
    {
        Task<SemesterDto> CreateAsync(CreateSemesterDto dto);
        Task<List<SemesterDto>> GetAllAsync();
        Task<SemesterDto?> GetByIdAsync(int id);
        Task<SemesterDto?> GetCurrentSemesterAsync();
        Task<bool> UpdateAsync(int id, UpdateSemesterDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}