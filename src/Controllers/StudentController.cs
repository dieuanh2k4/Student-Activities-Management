using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Students;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ApiControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentService _studentService;

        public StudentController(ApplicationDbContext context, IStudentService studentService, ILogger<StudentController> logger) : base(logger)
        {
            _context = context;
            _studentService = studentService;
        }

        [Authorize(Roles = "Admin")]  // Chỉ Admin xem tất cả students
        [HttpGet("get-all-student")]
        public async Task<IActionResult> GetAllStudent()
        {
            try
            {
                var student = await _studentService.GetAllStudent();

                return Ok(student);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [Authorize(Roles = "Admin")]  // Chỉ Admin tạo student
        [HttpPost("create-student")]
        public async Task<IActionResult> CreateStudent([FromForm] CreateStudentDto createStudentDto, int userid)
        {
            try
            {
                var newStudent = await _studentService.CreateStudent(createStudentDto, userid);

                await _context.Students.AddAsync(newStudent);
                await _context.SaveChangesAsync();

                return Ok(newStudent);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [Authorize]  // Student sửa profile mình, Admin sửa tất cả
        [HttpPut("update-student/{id}")]
        public async Task<IActionResult> UpdateInforStudent([FromForm] UpdateStudentDto updateStudentDto, int id)
        {
            try
            {
                var student = await _studentService.UpdateInforStudent(updateStudentDto, id);

                await _context.SaveChangesAsync();

                return Ok(student);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [Authorize(Roles = "Student,Admin")]  // Student xem điểm rèn luyện của mình
        [HttpGet("get-training-score")]
        public async Task<IActionResult> GetTrainingScore(int studentid)
        {
            try
            {
                var trainingScore = await _studentService.GetTrainingScore(studentid);

                return Ok(trainingScore);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [Authorize(Roles = "Student,Admin")]  // Student xem events đã tham gia
        [HttpGet("get-student-events")]
        public async Task<IActionResult> GetStudentEvents(int studentid)
        {
             try
            {
                var studentEvents = await _studentService.GetStudentEvents(studentid);

                return Ok(studentEvents);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}