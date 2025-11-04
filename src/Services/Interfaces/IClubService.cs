using StudentActivities.src.Dtos.Clubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IClubService
    {
        Task<ClubDto> CreateAsync(CreateClubDto dto);
        Task<List<ClubDto>> GetAllAsync();
        Task<ClubDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateClubDto dto);
        Task<bool> DeleteAsync(int id);
    }
}