using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentActivities.src.Dtos.Users;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class UserMapper
    {
        /// <summary>
        /// Chuyển đổi CreateUserDto thành Users entity với password đã hash
        /// </summary>
        public static async Task<Users> ToUserFromCreateUserDto(this CreateUserDto createUserDto)
        {
            return new Users
            {
                UserName = createUserDto.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password), // Hash password
                Role = createUserDto.Role
            };
        }
    }
}