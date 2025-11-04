using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Clubs;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Services.Implements
{
    public class ClubService : IClubService
    {
        private readonly ApplicationDbContext _context;
        public ClubService(ApplicationDbContext context) => _context = context;

        public async Task<ClubDto> CreateAsync(CreateClubDto dto)
        {
            var entity = new Clubs
            {
                Name = dto.Name,
                Thumbnail = dto.Thumbnail,
                Description = dto.Description,
                MaxCapacity = dto.MaxCapacity,
                OrganizerId = dto.OrganizerId
            };
            _context.Clubs.Add(entity);
            await _context.SaveChangesAsync();
            return ClubsMapper.ToDto(entity);
        }

        // ĐÃ SỬA: Include Organizers + Registrations
        public async Task<List<ClubDto>> GetAllAsync()
        {
            var list = await _context.Clubs
                .AsNoTracking()
                .Include(c => c.Organizers)           // Lấy tên người tổ chức
                .Include(c => c.Registrations)         // Đếm số thành viên
                .ToListAsync();

            return list.Select(ClubsMapper.ToDto).ToList();
        }

        // ĐÃ SỬA: Include Organizers + Registrations
        public async Task<ClubDto?> GetByIdAsync(int id)
        {
            var c = await _context.Clubs
                .AsNoTracking()
                .Include(c => c.Organizers)
                .Include(c => c.Registrations)
                .FirstOrDefaultAsync(x => x.Id == id);

            return c == null ? null : ClubsMapper.ToDto(c);
        }

        public async Task<bool> UpdateAsync(int id, UpdateClubDto dto)
        {
            var entity = await _context.Clubs
                .Include(c => c.Organizers) // Optional: nếu cần kiểm tra quyền
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return false;

            if (dto.Name != null) entity.Name = dto.Name;
            if (dto.Thumbnail != null) entity.Thumbnail = dto.Thumbnail;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.MaxCapacity.HasValue) entity.MaxCapacity = dto.MaxCapacity.Value;
            if (dto.OrganizerId.HasValue) entity.OrganizerId = dto.OrganizerId.Value;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Clubs.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return false;

            _context.Clubs.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}