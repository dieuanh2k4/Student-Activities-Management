using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Auth;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Đăng nhập và lấy JWT Token
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result == null)
                return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng." });

            return Ok(result);
        }

        /// <summary>
        /// Chỉ cho phép Admin truy cập
        /// </summary>
        [HttpGet("test-admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAdmin()
        {
            return Ok("Bạn đang đăng nhập với quyền Admin!");
        }

        /// <summary>
        /// Chỉ cho phép Student truy cập
        /// </summary>
        [HttpGet("test-student")]
        [Authorize(Roles = "Student")]
        public IActionResult TestStudent()
        {
            return Ok("Bạn đang đăng nhập với quyền Student!");
        }

        /// <summary>
        /// Chỉ cho phép Organizer truy cập
        /// </summary>
        [HttpGet("test-organizer")]
        [Authorize(Roles = "Organizer")]
        public IActionResult TestOrganizer()
        {
            return Ok("Bạn đang đăng nhập với quyền Organizer!");
        }

        /// <summary>
        /// Bất kỳ role nào đăng nhập đều được truy cập
        /// </summary>
        [HttpGet("test-any")]
        [Authorize]
        public IActionResult TestAnyRole()
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var username = User.Identity?.Name;

            return Ok(new
            {
                message = $"Xin chào {username}, bạn đang đăng nhập với quyền {role}."
            });
        }
    }
}
