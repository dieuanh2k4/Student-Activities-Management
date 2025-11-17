using System.Text;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Reports;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainingScoreReportDto>> GetScoreByPeriodAsync(ScoreByPeriodFilterDto filter)
        {
            var query = _context.TrainingScores
                .Include(ts => ts.Students)
                    .ThenInclude(s => s!.AcademicClasses)
                        .ThenInclude(ac => ac!.Faculties)
                .Include(ts => ts.Events)
                .Include(ts => ts.Semester)
                .AsQueryable();

            // Áp dụng filters
            if (filter.SemesterId.HasValue)
                query = query.Where(ts => ts.SemesterId == filter.SemesterId.Value);

            if (filter.ClassId.HasValue)
                query = query.Where(ts => ts.Students!.AcademicClassId == filter.ClassId.Value);

            if (filter.FacultyId.HasValue)
                query = query.Where(ts => ts.Students!.FacultyId == filter.FacultyId.Value);

            if (filter.FromDate.HasValue)
                query = query.Where(ts => ts.DateAssigned >= DateOnly.FromDateTime(filter.FromDate.Value));

            if (filter.ToDate.HasValue)
                query = query.Where(ts => ts.DateAssigned <= DateOnly.FromDateTime(filter.ToDate.Value));

            var scores = await query.ToListAsync();

            // Nhóm theo sinh viên
            var groupedByStudent = scores.GroupBy(ts => ts.StudentId);

            var reports = new List<TrainingScoreReportDto>();

            foreach (var group in groupedByStudent)
            {
                var student = group.First().Students;
                if (student == null) continue;

                var report = new TrainingScoreReportDto
                {
                    StudentId = student.Id,
                    StudentCode = $"SV{student.Id:D6}",
                    StudentName = $"{student.FirstName} {student.LastName}",
                    ClassName = student.AcademicClasses?.Name ?? "N/A",
                    FacultyName = student.AcademicClasses?.Faculties?.Name ?? "N/A",
                    TotalScore = group.Sum(ts => ts.Score),
                    EventCount = group.Count(),
                    Activities = group.Select(ts => new ActivityDetailDto
                    {
                        ActivityName = ts.Events?.Name ?? "N/A",
                        ActivityType = "Sự kiện",
                        Date = ts.DateAssigned.ToDateTime(TimeOnly.MinValue),
                        Score = ts.Score
                    }).ToList()
                };

                reports.Add(report);
            }

            return reports.OrderByDescending(r => r.TotalScore).ToList();
        }

        public async Task<TrainingScoreReportDto> GetScoreByActivityAsync(ScoreByActivityFilterDto filter)
        {
            var query = _context.TrainingScores
                .Include(ts => ts.Students)
                    .ThenInclude(s => s!.AcademicClasses)
                        .ThenInclude(ac => ac!.Faculties)
                .Include(ts => ts.Events)
                .Where(ts => ts.StudentId == filter.StudentId)
                .AsQueryable();

            // Filters cho sự kiện hoặc CLB
            if (filter.EventId.HasValue)
                query = query.Where(ts => ts.EventId == filter.EventId.Value);

            if (filter.FromDate.HasValue)
                query = query.Where(ts => ts.DateAssigned >= DateOnly.FromDateTime(filter.FromDate.Value));

            if (filter.ToDate.HasValue)
                query = query.Where(ts => ts.DateAssigned <= DateOnly.FromDateTime(filter.ToDate.Value));

            var scores = await query.ToListAsync();

            var student = scores.FirstOrDefault()?.Students;
            if (student == null)
            {
                throw new Exception("Không tìm thấy sinh viên");
            }

            // Lấy thông tin CLB nếu có filter ClubId
            List<ActivityDetailDto> clubActivities = new();
            if (filter.ClubId.HasValue)
            {
                var clubRegistrations = await _context.Resgistrations
                    .Include(r => r.Clubs)
                    .Include(r => r.Events)
                    .Where(r => r.StudentId == filter.StudentId && r.ClubId == filter.ClubId.Value)
                    .ToListAsync();

                clubActivities = clubRegistrations.Select(r => new ActivityDetailDto
                {
                    ActivityName = r.Clubs?.Name ?? r.Events?.Name ?? "N/A",
                    ActivityType = r.Clubs != null ? "Câu lạc bộ" : "Sự kiện",
                    Date = r.RegistrationDate,
                    Score = 0 // CLB không có điểm cụ thể, hoặc tính từ events
                }).ToList();
            }

            var eventActivities = scores.Select(ts => new ActivityDetailDto
            {
                ActivityName = ts.Events?.Name ?? "N/A",
                ActivityType = "Sự kiện",
                Date = ts.DateAssigned.ToDateTime(TimeOnly.MinValue),
                Score = ts.Score
            }).ToList();

            var allActivities = eventActivities.Concat(clubActivities).OrderByDescending(a => a.Date).ToList();

            var report = new TrainingScoreReportDto
            {
                StudentId = student.Id,
                StudentCode = $"SV{student.Id:D6}",
                StudentName = $"{student.FirstName} {student.LastName}",
                ClassName = student.AcademicClasses?.Name ?? "N/A",
                FacultyName = student.AcademicClasses?.Faculties?.Name ?? "N/A",
                TotalScore = eventActivities.Sum(a => a.Score),
                EventCount = allActivities.Count,
                Activities = allActivities
            };

            return report;
        }

        public async Task<byte[]> ExportScoreByPeriodToCsvAsync(ScoreByPeriodFilterDto filter)
        {
            var reports = await GetScoreByPeriodAsync(filter);

            var csv = new StringBuilder();

            // UTF-8 BOM để Excel hiển thị đúng tiếng Việt
            csv.Append("\uFEFF");

            // Header
            csv.AppendLine("Mã SV,Họ và tên,Lớp,Khoa,Tổng điểm,Số hoạt động");

            // Data rows
            foreach (var report in reports)
            {
                csv.AppendLine($"{report.StudentCode},{report.StudentName},{report.ClassName},{report.FacultyName},{report.TotalScore},{report.EventCount}");
            }

            // Chi tiết hoạt động cho từng sinh viên
            csv.AppendLine();
            csv.AppendLine("CHI TIẾT HOẠT ĐỘNG");
            csv.AppendLine();

            foreach (var report in reports)
            {
                csv.AppendLine($"Sinh viên: {report.StudentName} ({report.StudentCode})");
                csv.AppendLine("Tên hoạt động,Loại,Ngày,Điểm");

                foreach (var activity in report.Activities)
                {
                    csv.AppendLine($"{activity.ActivityName},{activity.ActivityType},{activity.Date:dd/MM/yyyy},{activity.Score}");
                }

                csv.AppendLine();
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        public async Task<byte[]> ExportScoreByActivityToCsvAsync(ScoreByActivityFilterDto filter)
        {
            var report = await GetScoreByActivityAsync(filter);

            var csv = new StringBuilder();

            // UTF-8 BOM
            csv.Append("\uFEFF");

            // Thông tin sinh viên
            csv.AppendLine($"Mã sinh viên:,{report.StudentCode}");
            csv.AppendLine($"Họ và tên:,{report.StudentName}");
            csv.AppendLine($"Lớp:,{report.ClassName}");
            csv.AppendLine($"Khoa:,{report.FacultyName}");
            csv.AppendLine($"Tổng điểm:,{report.TotalScore}");
            csv.AppendLine($"Số hoạt động:,{report.EventCount}");
            csv.AppendLine();

            // Chi tiết hoạt động
            csv.AppendLine("Tên hoạt động,Loại,Ngày,Điểm");
            foreach (var activity in report.Activities)
            {
                csv.AppendLine($"{activity.ActivityName},{activity.ActivityType},{activity.Date:dd/MM/yyyy},{activity.Score}");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }
    }
}
