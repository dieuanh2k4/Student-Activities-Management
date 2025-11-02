using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Constant;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Organizers;
using StudentActivities.src.Exceptions;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class OrganizerService : IOrganizerService
    {
        private static readonly List<Organizers> _organizer = new List<Organizers>();
        private readonly ApplicationDbContext _context;

        public OrganizerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Organizers>> GetAllOrganizer()
        {
            return await _context.Organizers
                .Include(s => s.Users)
                .ToListAsync();
        }

        public async Task<Organizers> CreateOrganizer(CreateOrganizerDto createOrganizerDto, int userid)
        {
            var role = UserTypes.Organizer;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid);

            if (user.Role != role)
            {
                throw new Result("Đây không phải tài khoản ban tổ chức");
            }

            var existingOrganizer = await _context.Organizers.FirstOrDefaultAsync(o => o.UserId == userid);

            if (existingOrganizer != null)
            {
                // Nếu đã tồn tại, ném ra một lỗi nghiệp vụ rõ ràng
                throw new Result("Tài khoản này đã được đăng ký làm ban tổ chức.");
            }

            var newOrganizer = await createOrganizerDto.ToOrganizerFromCreateDto(userid);
            return newOrganizer;
        }
        
        public async Task<Organizers> UpdateInforOrganizer([FromForm] UpdateOrganizerDto updateOrganizerDto, int id)
        {
            var organizer = await _context.Organizers.FindAsync(id);

            if (organizer == null)
            {
                throw new Result("Không tìm thấy người dùng cần chỉnh sửa");
            }

            organizer.FirstName = updateOrganizerDto.FirstName;
            organizer.LastName = updateOrganizerDto.LastName;
            organizer.PhoneNumber = updateOrganizerDto.PhoneNumber;
            organizer.Birth = updateOrganizerDto.Birth;
            organizer.Email = updateOrganizerDto.Email;

            return organizer;
        }
    }
}