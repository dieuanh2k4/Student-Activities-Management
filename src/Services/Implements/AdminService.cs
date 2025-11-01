using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Constant;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Admins;
using StudentActivities.src.Exceptions;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class AdminService : IAdminService
    {
        private static readonly List<Admins> _admin = new List<Admins>();
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Admins>> GetAllAdmin()
        {
            return await _context.Admins
                .Include(s => s.Users)
                .ToListAsync();
        }

        public async Task<Admins> CreateAdmin([FromForm] CreateAdminDto createAdminDto, int userid)
        {
            var role = UserTypes.Admin;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid);

            if (user.Role != role)
            {
                throw new Result("Đây không phải tài khoản Admin");
            }

            var existingAdmin = await _context.Admins.FirstOrDefaultAsync(o => o.UserId == userid);

            if (existingAdmin != null)
            {
                // Nếu đã tồn tại, ném ra một lỗi nghiệp vụ rõ ràng
                throw new Result("Tài khoản này đã được đăng ký làm Admin.");
            }
            
            var newAdmin = await createAdminDto.ToAdminsFromCreateDto(userid);
            return newAdmin;
        }
    }
}