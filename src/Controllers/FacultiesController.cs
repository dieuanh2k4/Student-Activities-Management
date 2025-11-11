using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Faculties;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]  // Global filter: Chỉ Admin quản lý khoa
    public class FacultiesController : ControllerBase
    {
        private readonly IFacultyService _facultyService;

        public FacultiesController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        /// <summary>
        /// Tạo khoa mới
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFacultyDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _facultyService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi tạo khoa", details = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả khoa (Public - ai cũng xem được)
        /// </summary>
        [AllowAnonymous]  // Override: Public endpoint
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var faculties = await _facultyService.GetAllAsync();
                return Ok(faculties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy danh sách khoa", details = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin khoa theo ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var faculty = await _facultyService.GetByIdAsync(id);
                if (faculty == null)
                    return NotFound(new { message = "Không tìm thấy khoa" });

                return Ok(faculty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy thông tin khoa", details = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin khoa
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFacultyDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _facultyService.UpdateAsync(id, dto);
                if (!success)
                    return NotFound(new { message = "Không tìm thấy khoa" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi cập nhật khoa", details = ex.Message });
            }
        }

        /// <summary>
        /// Xóa khoa
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _facultyService.DeleteAsync(id);
                if (!success)
                    return NotFound(new { message = "Không tìm thấy khoa" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi xóa khoa", details = ex.Message });
            }
        }
    }
}