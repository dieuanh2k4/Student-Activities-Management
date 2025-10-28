using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.AcademicClasses;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class AcademicClassService : IAcademicClassService
    {
        private readonly ApplicationDbContext _context;

        public AcademicClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AcademicClassDto> CreateAsync(CreateAcademicClassDto dto)
        {
            // Kiểm tra khoa tồn tại
            var facultyExists = await _context.Faculties.AnyAsync(f => f.Id == dto.FacultyId);
            if (!facultyExists)
                throw new InvalidOperationException("Khoa không tồn tại trong hệ thống");

            // Kiểm tra tên lớp đã tồn tại trong khoa
            var existingClass = await _context.AcademicClasses
                .FirstOrDefaultAsync(c => c.Name == dto.Name && c.FacultyId == dto.FacultyId);

            if (existingClass != null)
                throw new InvalidOperationException("Tên lớp đã tồn tại trong khoa này");

            var entity = AcademicClassMapper.ToEntity(dto);
            _context.AcademicClasses.Add(entity);
            await _context.SaveChangesAsync();

            // Load lại với Faculty để map đầy đủ
            await _context.Entry(entity)
                .Reference(c => c.Faculties)
                .LoadAsync();

            return AcademicClassMapper.ToDto(entity);
        }

        public async Task<List<AcademicClassDto>> GetAllAsync()
        {
            var academicClasses = await _context.AcademicClasses
                .Include(c => c.Faculties)
                .Include(c => c.Students)
                .AsNoTracking()
                .OrderBy(c => c.Faculties!.Name)
                .ThenBy(c => c.Name)
                .ToListAsync();

            return academicClasses.Select(AcademicClassMapper.ToDto).ToList();
        }

        public async Task<List<AcademicClassDto>> GetByFacultyIdAsync(int facultyId)
        {
            var academicClasses = await _context.AcademicClasses
                .Include(c => c.Faculties)
                .Include(c => c.Students)
                .Where(c => c.FacultyId == facultyId)
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();

            return academicClasses.Select(AcademicClassMapper.ToDto).ToList();
        }

        public async Task<AcademicClassDto?> GetByIdAsync(int id)
        {
            var academicClass = await _context.AcademicClasses
                .Include(c => c.Faculties)
                .Include(c => c.Students)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return academicClass == null ? null : AcademicClassMapper.ToDto(academicClass);
        }

        public async Task<bool> UpdateAsync(int id, UpdateAcademicClassDto dto)
        {
            var entity = await _context.AcademicClasses.FirstOrDefaultAsync(c => c.Id == id);
            if (entity == null) return false;

            // Kiểm tra khoa tồn tại nếu có cập nhật FacultyId
            if (dto.FacultyId.HasValue)
            {
                var facultyExists = await _context.Faculties.AnyAsync(f => f.Id == dto.FacultyId.Value);
                if (!facultyExists)
                    throw new InvalidOperationException("Khoa không tồn tại trong hệ thống");
            }

            // Kiểm tra tên lớp đã tồn tại trong khoa (trừ lớp hiện tại)
            if (!string.IsNullOrEmpty(dto.Name))
            {
                var facultyId = dto.FacultyId ?? entity.FacultyId;
                var existingClass = await _context.AcademicClasses
                    .FirstOrDefaultAsync(c => c.Name == dto.Name && c.FacultyId == facultyId && c.Id != id);

                if (existingClass != null)
                    throw new InvalidOperationException("Tên lớp đã tồn tại trong khoa này");
            }

            AcademicClassMapper.UpdateEntity(entity, dto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.AcademicClasses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null) return false;

            // Kiểm tra có sinh viên trong lớp không
            if (entity.Students?.Count > 0)
                throw new InvalidOperationException("Không thể xóa lớp vì còn sinh viên trong lớp này");

            _context.AcademicClasses.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.AcademicClasses.AnyAsync(c => c.Id == id);
        }
    }
}