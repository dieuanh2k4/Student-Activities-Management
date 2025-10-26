using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Users;
using StudentActivities.src.Exceptions;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class UserService : IUserService
    {
        private static readonly List<Users> _user = new List<Users>();
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> CreateUser([FromForm] CreateUserDto createUserDto)
        {
            var checkUsername = _user.FirstOrDefault(u => u.UserName.Equals(createUserDto.UserName, StringComparison.OrdinalIgnoreCase));

            if (checkUsername != null)
            {
                throw new Result($"Tên đăng nhập {createUserDto.UserName} đã tồn tại. Vui lòng thử tên đăng nhập khác");
            }

            var newUser = await createUserDto.ToUserFromCreateUserDto();

            return newUser;
        }

        public async Task<Users> UpdateUser([FromForm] UpdateUserDto updateUserDto, int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new Result("Không tìm thấy tài khoản cần chỉnh sửa");
            }

            user.UserName = updateUserDto.UserName;
            user.Password = updateUserDto.Password;
            user.Role = updateUserDto.Role;

            return user;
        }
        
        public async Task<Users> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new Result("Không tìm thấy user cần xóa");
            }
            else
            {
                return user;
            }
        }
    }
}