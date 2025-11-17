using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Users;
using StudentActivities.src.Models;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<Users>> GetAllUsers();
        Task<Users> CreateUser([FromForm] CreateUserDto createUserDto);
        Task<Users> UpdateUser([FromForm] UpdateUserDto updateUserDto, int id);
        Task<Users> DeleteUser(int id);
    }
}