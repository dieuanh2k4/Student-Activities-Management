using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Faculties;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class FacultyService : IFacultyService
    {
        private readonly ApplicationDbContext _context;

        public FacultyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FacultyDto> CreateAsync(CreateFacultyDto dto)
        {
            // Kiểm tra tên khoa đã tồn tại
            var existingFaculty = await _context.Faculties
                .FirstOrDefaultAsync(f => f.Name == dto.Name);

            if (existingFaculty != null)
                throw new InvalidOperationException("Tên khoa đã tồn tại trong hệ thống");

            var entity = FacultyMapper.ToEntity(dto);
            _context.Faculties.Add(entity);
            await _context.SaveChangesAsync();

            return FacultyMapper.ToDto(entity);
        }

        public async Task<List<FacultyDto>> GetAllAsync()
        {
            var faculties = await _context.Faculties
                .Include(f => f.AcademicClasses)
                    .ThenInclude(c => c.Students)
                .AsNoTracking()
                .OrderBy(f => f.Name)
                .ToListAsync();

            return faculties.Select(FacultyMapper.ToDto).ToList();
        }

        public async Task<FacultyDto?> GetByIdAsync(int id)
        {
            var faculty = await _context.Faculties
                .Include(f => f.AcademicClasses)
                    .ThenInclude(c => c.Students)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return faculty == null ? null : FacultyMapper.ToDto(faculty);
        }

        public async Task<bool> UpdateAsync(int id, UpdateFacultyDto dto)
        {
            var entity = await _context.Faculties.FirstOrDefaultAsync(f => f.Id == id);
            if (entity == null) return false;

            // Kiểm tra tên khoa đã tồn tại (trừ khoa hiện tại)
            if (!string.IsNullOrEmpty(dto.Name))
            {
                var existingFaculty = await _context.Faculties
                    .FirstOrDefaultAsync(f => f.Name == dto.Name && f.Id != id);

                if (existingFaculty != null)
                    throw new InvalidOperationException("Tên khoa đã tồn tại trong hệ thống");
            }

            FacultyMapper.UpdateEntity(entity, dto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Faculties
                .Include(f => f.AcademicClasses)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (entity == null) return false;

            // Kiểm tra có lớp học thuộc khoa này không
            if (entity.AcademicClasses?.Count > 0)
                throw new InvalidOperationException("Không thể xóa khoa vì còn lớp học thuộc khoa này");

            _context.Faculties.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Faculties.AnyAsync(f => f.Id == id);
        }
    }
}