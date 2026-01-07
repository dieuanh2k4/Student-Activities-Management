using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Semesters;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Admin")]  // Global filter: Chỉ Admin quản lý học kỳ
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemestersController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        /// <summary>
        /// Tạo học kỳ mới
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSemesterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _semesterService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi tạo học kỳ", details = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả học kỳ (Public - ai cũng xem được)
        /// </summary>
        [AllowAnonymous]  // Override: Public endpoint
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var semesters = await _semesterService.GetAllAsync();
                return Ok(semesters);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy danh sách học kỳ", details = ex.Message });
            }
        }

        /// <summary>
        /// Lấy học kỳ hiện tại
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent()
        {
            try
            {
                var currentSemester = await _semesterService.GetCurrentSemesterAsync();
                if (currentSemester == null)
                    return NotFound(new { message = "Không có học kỳ nào đang diễn ra" });

                return Ok(currentSemester);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy học kỳ hiện tại", details = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin học kỳ theo ID
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var semester = await _semesterService.GetByIdAsync(id);
                if (semester == null)
                    return NotFound(new { message = "Không tìm thấy học kỳ" });

                return Ok(semester);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy thông tin học kỳ", details = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin học kỳ
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSemesterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _semesterService.UpdateAsync(id, dto);
                if (!success)
                    return NotFound(new { message = "Không tìm thấy học kỳ" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi cập nhật học kỳ", details = ex.Message });
            }
        }

        /// <summary>
        /// Xóa học kỳ
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _semesterService.DeleteAsync(id);
                if (!success)
                    return NotFound(new { message = "Không tìm thấy học kỳ" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi xóa học kỳ", details = ex.Message });
            }
        }
    }
}