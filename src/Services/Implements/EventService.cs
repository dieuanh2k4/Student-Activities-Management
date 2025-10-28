using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Events;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EventDto> CreateAsync(CreateEventDto dto)
        {
            var entity = new Events
            {
                Name = dto.Name,
                Thumbnail = dto.Thumbnail,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Location = dto.Location,
                MaxCapacity = dto.MaxCapacity,
                CurrentRegistrations = 0,
                OrganizerId = dto.OrganizerId
            };

            _context.Events.Add(entity);
            await _context.SaveChangesAsync();

            return EventsMapper.ToDto(entity);
        }

        public async Task<List<EventDto>> GetAllAsync()
        {
            var list = await _context.Events
                                     .AsNoTracking()
                                     .OrderBy(e => e.StartDate)
                                     .ToListAsync();
            return list.Select(EventsMapper.ToDto).ToList();
        }

        public async Task<EventDto?> GetByIdAsync(int id)
        {
            var e = await _context.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return e == null ? null : EventsMapper.ToDto(e);
        }

        public async Task<bool> UpdateAsync(int id, UpdateEventDto dto)
        {
            var entity = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) return false;

            if (dto.Name != null) entity.Name = dto.Name;
            if (dto.Thumbnail != null) entity.Thumbnail = dto.Thumbnail;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.StartDate.HasValue) entity.StartDate = dto.StartDate.Value;
            if (dto.EndDate.HasValue) entity.EndDate = dto.EndDate.Value;
            if (dto.Location != null) entity.Location = dto.Location;
            if (dto.MaxCapacity.HasValue) entity.MaxCapacity = dto.MaxCapacity.Value;
            if (dto.CurrentRegistrations.HasValue) entity.CurrentRegistrations = dto.CurrentRegistrations.Value;
            if (dto.OrganizerId.HasValue) entity.OrganizerId = dto.OrganizerId.Value;

            _context.Events.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) return false;
            _context.Events.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}