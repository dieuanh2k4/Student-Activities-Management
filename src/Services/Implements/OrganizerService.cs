using System;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Events;
using StudentActivities.src.Dtos.Clubs;
using StudentActivities.src.Dtos.Organizers;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;
using StudentActivities.src.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace StudentActivities.src.Services.Implements
{
    public class OrganizerService : IOrganizerService
    {
        private static readonly List<Organizers> _organizer = new List<Organizers>();
        private readonly ApplicationDbContext _context;

        public OrganizerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Organizers>> GetAllOrganizer()
        {
            return await _context.Organizers.ToListAsync();
        }

        public async Task<Organizers> CreateOrganizer(CreateOrganizerDto createOrganizerDto, int userid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid);

            if (user.Role != "Organizer")
            {
                throw new Result("Đây không phải tài khoản ban tổ chức");
            }

            var existingOrganizer = await _context.Organizers.FirstOrDefaultAsync(o => o.UserId == userid);

            if (existingOrganizer != null)
            {
                // Nếu đã tồn tại, ném ra một lỗi nghiệp vụ rõ ràng
                throw new Result("Tài khoản này đã được đăng ký làm ban tổ chức.");
            }

            var newOrganizer = await createOrganizerDto.ToOrganizerFromCreateDto(userid);
            return newOrganizer;

        }
        
        public async Task<Organizers> UpdateInforOrganizer([FromForm] UpdateOrganizerDto updateOrganizerDto, int id)
        {
            var organizer = await _context.Organizers.FindAsync(id);

            if (organizer == null)
            {
                throw new Result("Không tìm thấy người dùng cần chỉnh sửa");
            }

            organizer.FirstName = updateOrganizerDto.FirstName;
            organizer.LastName = updateOrganizerDto.LastName;
            organizer.PhoneNumber = updateOrganizerDto.PhoneNumber;
            organizer.Birth = updateOrganizerDto.Birth;
            organizer.Email = updateOrganizerDto.Email;

            return organizer;
        }

        // 1. Xem dashboard
        public async Task<OrganizerDashboardDto?> GetDashboardAsync(int organizerId)
        {
            var organizer = await _context.Organizers
                .AsNoTracking()
                .Include(o => o.Clubs!)
                    .ThenInclude(c => c.Registrations)
                .Include(o => o.Events!)
                    .ThenInclude(ev => ev.Registrations)
                .FirstOrDefaultAsync(o => o.Id == organizerId);

            if (organizer == null) return null;

            return new OrganizerDashboardDto
            {

                Clubs = organizer.Clubs?.Select(ClubsMapper.ToSummaryDto).ToList() ?? new List<ClubSummaryDto>(),
                Events = organizer.Events?.Select(e => new EventSummaryDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    StartDate = e.StartDate,
                    Location = e.Location,
                    CurrentRegistrations = e.Registrations?.Count ?? 0,
                    MaxCapacity = e.MaxCapacity,
                    Status = e.Status
                }).ToList() ?? new List<EventSummaryDto>()
            };
        }

        // 2. Cập nhật sự kiện
        public async Task<bool> UpdateEventAsync(int organizerId, int eventId, UpdateEventDto dto)
        {
            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.OrganizerId == organizerId);

            if (@event == null) return false;

            if (dto.Name != null) @event.Name = dto.Name;
            if (dto.Thumbnail != null) @event.Thumbnail = dto.Thumbnail;
            if (dto.Description != null) @event.Description = dto.Description;
            if (dto.StartDate.HasValue) @event.StartDate = dto.StartDate.Value;
            if (dto.EndDate.HasValue) @event.EndDate = dto.EndDate.Value;
            if (dto.Location != null) @event.Location = dto.Location;
            if (dto.MaxCapacity.HasValue) @event.MaxCapacity = dto.MaxCapacity.Value;

            @event.UpdateDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        // 3. Xóa sự kiện
        public async Task<bool> DeleteEventAsync(int organizerId, int eventId)
        {
            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.OrganizerId == organizerId);

            if (@event == null) return false;

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return true;
        }

        // 4. Cập nhật câu lạc bộ
        public async Task<bool> UpdateClubAsync(int organizerId, int clubId, UpdateClubDto dto)
        {
            var club = await _context.Clubs
                .FirstOrDefaultAsync(c => c.Id == clubId && c.OrganizerId == organizerId);

            if (club == null) return false;

            if (dto.Name != null) club.Name = dto.Name;
            if (dto.Thumbnail != null) club.Thumbnail = dto.Thumbnail;
            if (dto.Description != null) club.Description = dto.Description;
            if (dto.MaxCapacity.HasValue) club.MaxCapacity = dto.MaxCapacity.Value;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}