using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Constant;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Students;
using StudentActivities.src.Exceptions;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class StudentService : IStudentService
    {
        private static readonly List<Students> _student = new List<Students>();
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Students>> GetAllStudent()
        {
            return await _context.Students
                .Include(s => s.Users)
                .Include(s => s.AcademicClasses)
                    .ThenInclude(ac => ac.Faculties)
                .ToListAsync();
        }

        public async Task<Students> CreateStudent(CreateStudentDto createStudentDto, int userid)
        {
            var role = UserTypes.Student;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid);

            if (user.Role != role)
            {
                throw new Result("Đây không phải tài khoản Sinh viên.");
            }

            var existingStudent = await _context.Students.FirstOrDefaultAsync(o => o.UserId == userid);

            if (existingStudent != null)
            {
                // Nếu đã tồn tại, ném ra một lỗi nghiệp vụ rõ ràng
                throw new Result("Tài khoản này đã được đăng ký làm sinh viên.");
            }

            var newStudent = await createStudentDto.ToStudentFromCreateDto(userid);
            return newStudent;
        }

        public async Task<Students> UpdateInforStudent([FromForm] UpdateStudentDto updateStudentDto, int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                throw new Result("Không tìm thấy người dùng cần chỉnh sửa");
            }

            student.FirstName = updateStudentDto.FirstName;
            student.LastName = updateStudentDto.LastName;
            student.PhoneNumber = updateStudentDto.PhoneNumber;
            student.Birth = updateStudentDto.Birth;
            student.Email = updateStudentDto.Email;
            student.AcademicClassId = updateStudentDto.AcademicClassId;
            student.FacultyId = updateStudentDto.FacultyId;

            return student;
        }

        public async Task<List<TrainingScores>> GetTrainingScore(int studentId)
        {
            var trainingScores = await _context.TrainingScores
                .Where(s => s.StudentId == studentId)
                .ToListAsync();

            if (trainingScores == null || !trainingScores.Any())
            {
                throw new Result("Không tìm thấy điểm rèn luyện cho sinh viên này");
            }

            return trainingScores;
        }
        
        public async Task<List<Events>> GetStudentEvents(int studentId)
        {
            var eventsId = await _context.Checkins
                .Where(c => c.StudentId == studentId)
                .Select(c => c.EventId)
                .ToListAsync();

            if (eventsId == null || !eventsId.Any())
            {
                throw new Result("Sinh viên chưa tham gia sự kiện nào");
            }

            var eventsStudent = await _context.Events
                .Where(e => eventsId.Contains(e.Id))
                .ToListAsync();

            return eventsStudent;
        }
    }
}