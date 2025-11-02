using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentActivities.src.Dtos.Students;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class StudentMapper
    {
        public static async Task<Students> ToStudentFromCreateDto(this CreateStudentDto createStudentDto, int userid)
        {
            return new Students
            {
                FirstName = createStudentDto.FirstName,
                LastName = createStudentDto.LastName,
                PhoneNumber = createStudentDto.PhoneNumber,
                Birth = createStudentDto.Birth,
                Email = createStudentDto.Email,
                AcademicClassId = createStudentDto.AcademicClassId,
                FacultyId = createStudentDto.FacultyId,
                UserId = userid
            };
        }
    }
}