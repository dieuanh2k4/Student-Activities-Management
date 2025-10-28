using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.AcademicClasses;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcademicClassesController : ControllerBase
    {
        private readonly IAcademicClassService _academicClassService;

        public AcademicClassesController(IAcademicClassService academicClassService)
        {
            _academicClassService = academicClassService;
        }

        /// <summary>
        /// Tạo lớp học mới
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAcademicClassDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _academicClassService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi tạo lớp học", details = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả lớp học
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var academicClasses = await _academicClassService.GetAllAsync();
                return Ok(academicClasses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy danh sách lớp học", details = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách lớp học theo khoa
        /// </summary>
        [HttpGet("faculty/{facultyId:int}")]
        public async Task<IActionResult> GetByFacultyId(int facultyId)
        {
            try
            {
                var academicClasses = await _academicClassService.GetByFacultyIdAsync(facultyId);
                return Ok(academicClasses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy danh sách lớp học theo khoa", details = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin lớp học theo ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var academicClass = await _academicClassService.GetByIdAsync(id);
                if (academicClass == null)
                    return NotFound(new { message = "Không tìm thấy lớp học" });

                return Ok(academicClass);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy thông tin lớp học", details = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin lớp học
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAcademicClassDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _academicClassService.UpdateAsync(id, dto);
                if (!success)
                    return NotFound(new { message = "Không tìm thấy lớp học" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi cập nhật lớp học", details = ex.Message });
            }
        }

        /// <summary>
        /// Xóa lớp học
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _academicClassService.DeleteAsync(id);
                if (!success)
                    return NotFound(new { message = "Không tìm thấy lớp học" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi xóa lớp học", details = ex.Message });
            }
        }
    }
}