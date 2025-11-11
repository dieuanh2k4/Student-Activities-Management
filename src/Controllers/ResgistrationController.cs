using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Resgistrations;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResgistrationController : ApiControllerBase
    {
        private readonly IResgistrationService _resgistrationService;

        public ResgistrationController(IResgistrationService resgistrationService, ILogger<ResgistrationController> logger) 
            : base(logger)
        {
            _resgistrationService = resgistrationService;
        }

        // Helper method để lấy StudentId từ JWT token
        private int GetCurrentStudentId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Không thể xác định người dùng");
            }
            return userId;
        }

        /// <summary>
        /// Lấy danh sách sự kiện/CLB có thể đăng ký (cần đăng nhập)
        /// </summary>
        /// <param name="type">Lọc theo loại: EVENT hoặc CLUB (optional)</param>
        [Authorize]  // Phải đăng nhập
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableActivities([FromQuery] string? type = null)
        {
            try
            {
                var studentId = GetCurrentStudentId();
                var result = await _resgistrationService.GetAvailableActivitiesAsync(studentId, type);

                if (result.IsSuccess)
                    return Ok(result.Data);

                return BadRequest(new { message = result.Error });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Đăng ký tham gia sự kiện/CLB (Student)
        /// </summary>
        [Authorize(Roles = "Student,Admin")]  // Student đăng ký, Admin test
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateResgistrationDto dto)
        {
            try
            {
                var studentId = GetCurrentStudentId();
                var result = await _resgistrationService.RegisterActivityAsync(studentId, dto);

                if (result.IsSuccess)
                    return Ok(new 
                    { 
                        message = "Đăng ký thành công",
                        data = result.Data 
                    });

                return BadRequest(new { message = result.Error });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Hủy đăng ký sự kiện/CLB (Student)
        /// </summary>
        /// <param name="activityId">ID của sự kiện/CLB</param>
        /// <param name="type">Loại: EVENT hoặc CLUB</param>
        [Authorize(Roles = "Student,Admin")]  // Student hủy, Admin test
        [HttpDelete("cancel/{activityId}")]
        public async Task<IActionResult> Cancel(int activityId, [FromQuery] string type)
        {
            try
            {
                var studentId = GetCurrentStudentId();
                var result = await _resgistrationService.CancelRegistrationAsync(studentId, activityId, type);

                if (result.IsSuccess)
                    return Ok(new { message = "Hủy đăng ký thành công" });

                return BadRequest(new { message = result.Error });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách đăng ký của sinh viên (Student, Admin, Organizer)
        /// </summary>
        /// <param name="type">Lọc theo loại: EVENT hoặc CLUB (optional)</param>
        /// <param name="status">Lọc theo trạng thái: REGISTERED, CANCELLED, etc (optional)</param>
        [Authorize]  // Phải đăng nhập
        [HttpGet("my-registrations")]
        public async Task<IActionResult> GetMyRegistrations([FromQuery] string? type = null, [FromQuery] string? status = null)
        {
            try
            {
                var studentId = GetCurrentStudentId();
                var result = await _resgistrationService.GetMyRegistrationsAsync(studentId, type, status);

                if (result.IsSuccess)
                    return Ok(result.Data);

                return BadRequest(new { message = result.Error });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết đăng ký cụ thể
        /// </summary>
        /// <param name="activityId">ID của sự kiện/CLB</param>
        /// <param name="type">Loại: EVENT hoặc CLUB</param>
        [HttpGet("detail/{activityId}")]
        public async Task<IActionResult> GetRegistrationDetail(int activityId, [FromQuery] string type)
        {
            try
            {
                var studentId = GetCurrentStudentId();
                var result = await _resgistrationService.GetRegistrationDetailAsync(studentId, activityId, type);

                if (result.IsSuccess)
                    return Ok(result.Data);

                return BadRequest(new { message = result.Error });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}
