using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Students;
using StudentActivities.src.Models;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<Students>> GetAllStudent();
        Task<Students> CreateStudent(CreateStudentDto createStudentDto, int userid);
        Task<Students> UpdateInforStudent([FromForm] UpdateStudentDto updateStudentDto, int id);
    }
}