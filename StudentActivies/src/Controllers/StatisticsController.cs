using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Statistics;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Yêu cầu authentication
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ILogger<StatisticsController> _logger;

        public StatisticsController(
            IStatisticsService statisticsService,
            ILogger<StatisticsController> logger)
        {
            _statisticsService = statisticsService;
            _logger = logger;
        }

        /// <summary>
        /// Thống kê sinh viên theo lớp, khoa, kỳ
        /// </summary>
        /// <param name="filter">Bộ lọc (ClassId, FacultyId, SemesterId)</param>
        /// <returns>Thống kê sinh viên</returns>
        [HttpGet("students")]
        [ProducesResponseType(typeof(StudentStatisticsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudentStatistics([FromQuery] StudentStatisticsFilterDto? filter)
        {
            try
            {
                var result = await _statisticsService.GetStudentStatisticsAsync(filter);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "Lấy thống kê sinh viên thành công"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student statistics");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi lấy thống kê sinh viên"
                });
            }
        }

        /// <summary>
        /// Thống kê sự kiện theo ngày/tháng/năm
        /// </summary>
        /// <param name="filter">Bộ lọc (StartDate, EndDate, Period: daily/monthly/yearly)</param>
        /// <returns>Thống kê sự kiện</returns>
        [HttpGet("events/by-date")]
        [ProducesResponseType(typeof(EventStatisticsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventStatisticsByDate([FromQuery] EventStatisticsFilterDto filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.Period))
                    filter.Period = "daily";

                var result = await _statisticsService.GetEventStatisticsByDateAsync(filter);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "Lấy thống kê sự kiện theo thời gian thành công"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting event statistics by date");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi lấy thống kê sự kiện"
                });
            }
        }

        /// <summary>
        /// Thống kê câu lạc bộ theo ngày/tháng/năm
        /// </summary>
        /// <param name="filter">Bộ lọc (StartDate, EndDate, Period: daily/monthly/yearly)</param>
        /// <returns>Thống kê câu lạc bộ</returns>
        [HttpGet("clubs/by-date")]
        [ProducesResponseType(typeof(ClubStatisticsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClubStatisticsByDate([FromQuery] ClubStatisticsFilterDto filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.Period))
                    filter.Period = "monthly";

                var result = await _statisticsService.GetClubStatisticsByDateAsync(filter);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "Lấy thống kê câu lạc bộ theo thời gian thành công"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting club statistics by date");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi lấy thống kê câu lạc bộ"
                });
            }
        }

        /// <summary>
        /// Thống kê sự kiện theo trạng thái
        /// </summary>
        /// <param name="filter">Bộ lọc (Status, FromDate, ToDate, OrganizerId)</param>
        /// <returns>Thống kê sự kiện theo trạng thái</returns>
        [HttpGet("events/by-status")]
        [ProducesResponseType(typeof(EventStatusStatisticsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventStatisticsByStatus([FromQuery] EventStatusFilterDto? filter)
        {
            try
            {
                var result = await _statisticsService.GetEventStatisticsByStatusAsync(filter);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "Lấy thống kê sự kiện theo trạng thái thành công"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting event statistics by status");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi lấy thống kê sự kiện theo trạng thái"
                });
            }
        }

        /// <summary>
        /// Dashboard tổng quan - Tổng hợp tất cả thống kê
        /// </summary>
        /// <returns>Dashboard data</returns>
        [HttpGet("dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var studentStats = await _statisticsService.GetStudentStatisticsAsync();
                var eventStats = await _statisticsService.GetEventStatisticsByDateAsync(new EventStatisticsFilterDto
                {
                    Period = "monthly",
                    StartDate = DateTime.Now.AddMonths(-6),
                    EndDate = DateTime.Now
                });
                var eventStatusStats = await _statisticsService.GetEventStatisticsByStatusAsync();

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        students = studentStats,
                        events = eventStats,
                        eventStatus = eventStatusStats
                    },
                    message = "Lấy dữ liệu dashboard thành công"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi lấy dữ liệu dashboard"
                });
            }
        }
    }
}