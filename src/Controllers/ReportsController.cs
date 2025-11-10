using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Reports;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ApiControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
            : base(logger)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Báo cáo điểm rèn luyện theo kỳ/lớp/khoa
        /// </summary>
        [HttpPost("training-scores/by-period")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetScoreByPeriod([FromBody] ScoreByPeriodFilterDto filter)
        {
            try
            {
                var reports = await _reportService.GetScoreByPeriodAsync(filter);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Báo cáo điểm rèn luyện theo sự kiện/CLB của sinh viên
        /// </summary>
        [HttpPost("training-scores/by-activity")]
        [Authorize(Roles = "Admin,Organizer,Student")]
        public async Task<IActionResult> GetScoreByActivity([FromBody] ScoreByActivityFilterDto filter)
        {
            try
            {
                var report = await _reportService.GetScoreByActivityAsync(filter);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Xuất báo cáo điểm rèn luyện theo kỳ/lớp/khoa ra file CSV
        /// </summary>
        [HttpPost("training-scores/by-period/export-csv")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> ExportScoreByPeriodToCsv([FromBody] ScoreByPeriodFilterDto filter)
        {
            try
            {
                var csvBytes = await _reportService.ExportScoreByPeriodToCsvAsync(filter);

                var fileName = $"BaoCaoDiemRenLuyen_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                return File(csvBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Xuất báo cáo điểm rèn luyện theo hoạt động của sinh viên ra file CSV
        /// </summary>
        [HttpPost("training-scores/by-activity/export-csv")]
        [Authorize(Roles = "Admin,Organizer,Student")]
        public async Task<IActionResult> ExportScoreByActivityToCsv([FromBody] ScoreByActivityFilterDto filter)
        {
            try
            {
                var csvBytes = await _reportService.ExportScoreByActivityToCsvAsync(filter);

                var fileName = $"BaoCaoDiemRenLuyen_SV{filter.StudentId}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                return File(csvBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy tổng quan điểm rèn luyện theo khoa
        /// </summary>
        [HttpGet("training-scores/summary/by-faculty")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetScoreSummaryByFaculty([FromQuery] int? semesterId)
        {
            try
            {
                var filter = new ScoreByPeriodFilterDto
                {
                    SemesterId = semesterId
                };

                var reports = await _reportService.GetScoreByPeriodAsync(filter);

                // Nhóm theo khoa
                var summary = reports
                    .GroupBy(r => r.FacultyName)
                    .Select(g => new
                    {
                        FacultyName = g.Key,
                        StudentCount = g.Count(),
                        TotalScore = g.Sum(r => r.TotalScore),
                        AverageScore = g.Average(r => r.TotalScore),
                        MaxScore = g.Max(r => r.TotalScore),
                        MinScore = g.Min(r => r.TotalScore)
                    })
                    .OrderByDescending(s => s.AverageScore)
                    .ToList();

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy tổng quan điểm rèn luyện theo lớp
        /// </summary>
        [HttpGet("training-scores/summary/by-class")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetScoreSummaryByClass([FromQuery] int? facultyId, [FromQuery] int? semesterId)
        {
            try
            {
                var filter = new ScoreByPeriodFilterDto
                {
                    FacultyId = facultyId,
                    SemesterId = semesterId
                };

                var reports = await _reportService.GetScoreByPeriodAsync(filter);

                // Nhóm theo lớp
                var summary = reports
                    .GroupBy(r => r.ClassName)
                    .Select(g => new
                    {
                        ClassName = g.Key,
                        FacultyName = g.First().FacultyName,
                        StudentCount = g.Count(),
                        TotalScore = g.Sum(r => r.TotalScore),
                        AverageScore = g.Average(r => r.TotalScore),
                        MaxScore = g.Max(r => r.TotalScore),
                        MinScore = g.Min(r => r.TotalScore)
                    })
                    .OrderByDescending(s => s.AverageScore)
                    .ToList();

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}
