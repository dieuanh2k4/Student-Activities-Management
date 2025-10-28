using System.Collections.Generic;
using System.Threading.Tasks;
using StudentActivities.src.Dtos.Events;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IEventService
    {
        Task<EventDto> CreateAsync(CreateEventDto dto);
        Task<List<EventDto>> GetAllAsync();
        Task<EventDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateEventDto dto);
        Task<bool> DeleteAsync(int id);
    }
}