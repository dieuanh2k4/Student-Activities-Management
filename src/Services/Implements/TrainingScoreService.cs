using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.TrainingScores;
using StudentActivities.src.Mappers;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class TrainingScoreService : ITrainingScoreService
    {
        private readonly ApplicationDbContext _context;

        public TrainingScoreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedTrainingScoreDto> GetAllTrainingScoresAsync(TrainingScoreFilterDto filter)
        {
            var query = _context.TrainingScores
                .Include(ts => ts.Events)
                .Include(ts => ts.Students)
                    .ThenInclude(s => s!.Users)
                .Include(ts => ts.Students)
                    .ThenInclude(s => s!.AcademicClasses)
                        .ThenInclude(ac => ac!.Faculties)
                .Include(ts => ts.Semester)
                .AsQueryable();

            // Apply filters
            if (filter.StudentId.HasValue)
            {
                query = query.Where(ts => ts.StudentId == filter.StudentId.Value);
            }

            if (filter.EventId.HasValue)
            {
                query = query.Where(ts => ts.EventId == filter.EventId.Value);
            }

            if (filter.SemesterId.HasValue)
            {
                query = query.Where(ts => ts.SemesterId == filter.SemesterId.Value);
            }

            if (filter.FacultyId.HasValue)
            {
                query = query.Where(ts => ts.Students!.FacultyId == filter.FacultyId.Value);
            }

            if (filter.ClassId.HasValue)
            {
                query = query.Where(ts => ts.Students!.AcademicClassId == filter.ClassId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.StudentCode))
            {
                query = query.Where(ts => ts.Students!.Users!.UserName!.Contains(filter.StudentCode));
            }

            if (!string.IsNullOrWhiteSpace(filter.StudentName))
            {
                query = query.Where(ts =>
                    (ts.Students!.FirstName + " " + ts.Students.LastName).Contains(filter.StudentName));
            }

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .OrderByDescending(ts => ts.DateAssigned)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(ts => ts.ToDto())
                .ToListAsync();

            return new PagedTrainingScoreDto
            {
                Items = items,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };
        }

        public async Task<TrainingScoreDto?> GetTrainingScoreByIdAsync(int id)
        {
            var trainingScore = await _context.TrainingScores
                .Include(ts => ts.Events)
                .Include(ts => ts.Students)
                    .ThenInclude(s => s!.Users)
                .Include(ts => ts.Students)
                    .ThenInclude(s => s!.AcademicClasses)
                        .ThenInclude(ac => ac!.Faculties)
                .Include(ts => ts.Semester)
                .FirstOrDefaultAsync(ts => ts.Id == id);

            return trainingScore?.ToDto();
        }

        public async Task<TrainingScoreDto> UpdateTrainingScoreAsync(int id, UpdateTrainingScoreDto dto)
        {
            var trainingScore = await _context.TrainingScores.FindAsync(id);

            if (trainingScore == null)
            {
                throw new Exception($"Không tìm thấy điểm rèn luyện với Id = {id}");
            }

            trainingScore.Score = dto.Score;

            await _context.SaveChangesAsync();

            // Reload with includes
            return (await GetTrainingScoreByIdAsync(id))!;
        }

        // Chức năng 2: Cập nhật điểm rèn luyện hàng loạt cho sự kiện
        public async Task<BulkUpdateResultDto> UpdateEventTrainingPointsAsync(int eventId, UpdateEventTrainingPointsDto dto)
        {
            // Kiểm tra sự kiện có tồn tại không
            var eventEntity = await _context.Events.FindAsync(eventId);
            if (eventEntity == null)
            {
                throw new Exception($"Không tìm thấy sự kiện với Id = {eventId}");
            }

            // Lấy danh sách sinh viên đã tham dự (từ bảng Checkin)
            var attendedStudentIds = await _context.Checkins
                .Where(c => c.EventId == eventId && c.Status == "Đã tham dự")
                .Select(c => c.StudentId)
                .Distinct()
                .ToListAsync();

            if (!attendedStudentIds.Any())
            {
                return new BulkUpdateResultDto
                {
                    TotalStudents = 0,
                    UpdatedCount = 0,
                    CreatedCount = 0,
                    Message = "Không có sinh viên nào đã tham dự sự kiện này"
                };
            }

            int updatedCount = 0;
            int createdCount = 0;

            // Cập nhật hoặc tạo mới TrainingScores cho từng sinh viên
            foreach (var studentId in attendedStudentIds)
            {
                var existingScore = await _context.TrainingScores
                    .FirstOrDefaultAsync(ts =>
                        ts.StudentId == studentId &&
                        ts.EventId == eventId &&
                        ts.SemesterId == dto.SemesterId);

                if (existingScore != null)
                {
                    // Cập nhật điểm cũ
                    existingScore.Score = dto.Score;
                    updatedCount++;
                }
                else
                {
                    // Tạo record mới
                    var newScore = new Models.TrainingScores
                    {
                        StudentId = studentId,
                        EventId = eventId,
                        SemesterId = dto.SemesterId,
                        Score = dto.Score,
                        DateAssigned = DateOnly.FromDateTime(DateTime.UtcNow)
                    };
                    _context.TrainingScores.Add(newScore);
                    createdCount++;
                }
            }

            // Note: Model Events không có TrainingPoints property
            // Nếu cần lưu điểm vào Event, cần thêm property TrainingPoints vào model Events

            await _context.SaveChangesAsync();

            return new BulkUpdateResultDto
            {
                TotalStudents = attendedStudentIds.Count,
                UpdatedCount = updatedCount,
                CreatedCount = createdCount,
                Message = $"Đã cập nhật điểm rèn luyện thành công cho {attendedStudentIds.Count} sinh viên"
            };
        }

        // Chức năng 2: Cập nhật điểm rèn luyện hàng loạt cho CLB
        public async Task<BulkUpdateResultDto> UpdateClubTrainingPointsAsync(int clubId, UpdateClubTrainingPointsDto dto)
        {
            // Kiểm tra CLB có tồn tại không
            var club = await _context.Clubs.FindAsync(clubId);
            if (club == null)
            {
                throw new Exception($"Không tìm thấy câu lạc bộ với Id = {clubId}");
            }

            // Lấy danh sách sinh viên đã tham gia CLB (từ bảng Resgistrations)
            // Note: Model Resgistrations không có Type và Status cụ thể
            // Chỉ lấy những sinh viên đã đăng ký CLB này
            var memberStudentIds = await _context.Resgistrations
                .Where(r => r.ClubId == clubId)
                .Select(r => r.StudentId)
                .Distinct()
                .ToListAsync();

            if (!memberStudentIds.Any())
            {
                return new BulkUpdateResultDto
                {
                    TotalStudents = 0,
                    UpdatedCount = 0,
                    CreatedCount = 0,
                    Message = "Không có sinh viên nào tham gia câu lạc bộ này"
                };
            }

            int updatedCount = 0;
            int createdCount = 0;

            // Cập nhật hoặc tạo mới TrainingScores cho từng sinh viên
            foreach (var studentId in memberStudentIds)
            {
                var existingScore = await _context.TrainingScores
                    .FirstOrDefaultAsync(ts =>
                        ts.StudentId == studentId &&
                        ts.EventId == clubId && // Dùng EventId để lưu ClubId (cần refactor model nếu muốn tách riêng)
                        ts.SemesterId == dto.SemesterId);

                if (existingScore != null)
                {
                    // Cập nhật điểm cũ
                    existingScore.Score = dto.Score;
                    updatedCount++;
                }
                else
                {
                    // Tạo record mới
                    var newScore = new Models.TrainingScores
                    {
                        StudentId = studentId,
                        EventId = clubId, // Lưu tạm ClubId vào EventId
                        SemesterId = dto.SemesterId,
                        Score = dto.Score,
                        DateAssigned = DateOnly.FromDateTime(DateTime.UtcNow)
                    };
                    _context.TrainingScores.Add(newScore);
                    createdCount++;
                }
            }

            await _context.SaveChangesAsync();

            return new BulkUpdateResultDto
            {
                TotalStudents = memberStudentIds.Count(),
                UpdatedCount = updatedCount,
                CreatedCount = createdCount,
                Message = $"Đã cập nhật điểm rèn luyện thành công cho {memberStudentIds.Count()} thành viên CLB"
            };
        }
    }
}
