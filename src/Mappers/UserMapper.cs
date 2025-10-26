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
        public static async Task<Users> ToUserFromCreateUserDto(this CreateUserDto createUserDto)
        {
            return new Users
            {
                UserName = createUserDto.UserName,
                Password = createUserDto.Password,
                Role = createUserDto.Role
            };
        }
    }
}