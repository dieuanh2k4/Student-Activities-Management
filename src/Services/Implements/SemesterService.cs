using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Semesters;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class SemesterService : ISemesterService
    {
        private readonly ApplicationDbContext _context;

        public SemesterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SemesterDto> CreateAsync(CreateSemesterDto dto)
        {
            // Validate ngày
            if (dto.EndDate <= dto.StartDate)
                throw new InvalidOperationException("Ngày kết thúc phải sau ngày bắt đầu");

            // Kiểm tra tên học kỳ đã tồn tại
            var existingSemester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.Name == dto.Name);

            if (existingSemester != null)
                throw new InvalidOperationException("Tên học kỳ đã tồn tại trong hệ thống");

            // Kiểm tra trùng lặp thời gian
            var overlappingSemester = await _context.Semesters
                .FirstOrDefaultAsync(s =>
                    (dto.StartDate >= s.StartDate && dto.StartDate <= s.EndDate) ||
                    (dto.EndDate >= s.StartDate && dto.EndDate <= s.EndDate) ||
                    (dto.StartDate <= s.StartDate && dto.EndDate >= s.EndDate));

            if (overlappingSemester != null)
                throw new InvalidOperationException($"Thời gian học kỳ bị trùng với học kỳ '{overlappingSemester.Name}'");

            var entity = SemesterMapper.ToEntity(dto);
            _context.Semesters.Add(entity);
            await _context.SaveChangesAsync();

            return SemesterMapper.ToDto(entity);
        }

        public async Task<List<SemesterDto>> GetAllAsync()
        {
            var semesters = await _context.Semesters
                .AsNoTracking()
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();

            return semesters.Select(SemesterMapper.ToDto).ToList();
        }

        public async Task<SemesterDto?> GetByIdAsync(int id)
        {
            var semester = await _context.Semesters
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return semester == null ? null : SemesterMapper.ToDto(semester);
        }

        public async Task<SemesterDto?> GetCurrentSemesterAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var currentSemester = await _context.Semesters
                .AsNoTracking()
                .FirstOrDefaultAsync(s => today >= s.StartDate && today <= s.EndDate);

            return currentSemester == null ? null : SemesterMapper.ToDto(currentSemester);
        }

        public async Task<bool> UpdateAsync(int id, UpdateSemesterDto dto)
        {
            var entity = await _context.Semesters.FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null) return false;

            // Validate ngày nếu có cập nhật
            var startDate = dto.StartDate ?? entity.StartDate;
            var endDate = dto.EndDate ?? entity.EndDate;

            if (endDate <= startDate)
                throw new InvalidOperationException("Ngày kết thúc phải sau ngày bắt đầu");

            // Kiểm tra tên học kỳ đã tồn tại (trừ học kỳ hiện tại)
            if (!string.IsNullOrEmpty(dto.Name))
            {
                var existingSemester = await _context.Semesters
                    .FirstOrDefaultAsync(s => s.Name == dto.Name && s.Id != id);

                if (existingSemester != null)
                    throw new InvalidOperationException("Tên học kỳ đã tồn tại trong hệ thống");
            }

            // Kiểm tra trùng lặp thời gian (trừ học kỳ hiện tại)
            if (dto.StartDate.HasValue || dto.EndDate.HasValue)
            {
                var overlappingSemester = await _context.Semesters
                    .FirstOrDefaultAsync(s => s.Id != id &&
                        ((startDate >= s.StartDate && startDate <= s.EndDate) ||
                         (endDate >= s.StartDate && endDate <= s.EndDate) ||
                         (startDate <= s.StartDate && endDate >= s.EndDate)));

                if (overlappingSemester != null)
                    throw new InvalidOperationException($"Thời gian học kỳ bị trùng với học kỳ '{overlappingSemester.Name}'");
            }

            SemesterMapper.UpdateEntity(entity, dto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Semesters
                .Include(s => s.TrainingScores)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (entity == null) return false;

            // Kiểm tra có điểm rèn luyện trong học kỳ này không
            if (entity.TrainingScores?.Count > 0)
                throw new InvalidOperationException("Không thể xóa học kỳ vì còn điểm rèn luyện trong học kỳ này");

            _context.Semesters.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Semesters.AnyAsync(s => s.Id == id);
        }
    }
}