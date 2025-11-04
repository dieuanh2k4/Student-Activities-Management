using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Checkins;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/events/{eventId}/checkin")]
    public class CheckinController : ApiControllerBase
    {
        private readonly ICheckinService _checkinService;

        public CheckinController(ICheckinService checkinService, ILogger<CheckinController> logger)
            : base(logger)
        {
            _checkinService = checkinService;
        }

        /// <summary>
        /// Chức năng 1: Lấy danh sách sinh viên đã đăng ký sự kiện
        /// GET: api/events/{eventId}/checkin/registrations
        /// </summary>
        [HttpGet("registrations")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetEventRegistrations([FromRoute] int eventId)
        {
            try
            {
                var registrations = await _checkinService.GetEventRegistrationsAsync(eventId);
                return Ok(registrations);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Chức năng 2: Xem trạng thái check-in của sinh viên trong sự kiện
        /// GET: api/events/{eventId}/checkin
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetEventCheckins([FromRoute] int eventId)
        {
            try
            {
                var checkins = await _checkinService.GetEventCheckinsAsync(eventId);
                return Ok(checkins);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Chức năng 3: Cập nhật trạng thái check-in cho sinh viên (Manual)
        /// PUT: api/events/{eventId}/checkin/{studentId}
        /// </summary>
        [HttpPut("{studentId}")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> UpdateCheckinStatus(
            [FromRoute] int eventId,
            [FromRoute] int studentId,
            [FromBody] UpdateCheckinStatusDto dto)
        {
            try
            {
                // Lấy UserId từ JWT token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var userId = int.Parse(userIdClaim.Value);

                var result = await _checkinService.UpdateCheckinStatusAsync(eventId, studentId, dto, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sinh viên trong sự kiện (hỗ trợ check-in thủ công)
        /// GET: api/events/{eventId}/checkin/search?query=...
        /// </summary>
        [HttpGet("search")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> SearchStudentsInEvent(
            [FromRoute] int eventId,
            [FromQuery] string query)
        {
            try
            {
                var students = await _checkinService.SearchStudentsInEventAsync(eventId, query);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy thống kê check-in của sự kiện
        /// GET: api/events/{eventId}/checkin/statistics
        /// </summary>
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetCheckinStatistics([FromRoute] int eventId)
        {
            try
            {
                var statistics = await _checkinService.GetCheckinStatisticsAsync(eventId);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Check-in hàng loạt (Admin only)
        /// POST: api/events/{eventId}/checkin/bulk
        /// </summary>
        [HttpPost("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BulkCheckin(
            [FromRoute] int eventId,
            [FromBody] List<int> studentIds)
        {
            try
            {
                // Lấy UserId từ JWT token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var userId = int.Parse(userIdClaim.Value);

                var result = await _checkinService.BulkCheckinAsync(eventId, studentIds, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}
