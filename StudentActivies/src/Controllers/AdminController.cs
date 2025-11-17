using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Admins;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]  // Global filter: Chỉ Admin mới truy cập
    public class AdminController : ApiControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminService _adminService;

        public AdminController(ApplicationDbContext context, IAdminService adminService, ILogger<OrganizerController> logger) : base(logger)
        {
            _context = context;
            _adminService = adminService;
        }

        [HttpGet("get-all-admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            try
            {
                var admin = await _adminService.GetAllAdmin();

                return Ok(admin);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromForm] CreateAdminDto createAdminDto, int userid)
        {
            try
            {
                var newAdmin = await _adminService.CreateAdmin(createAdminDto, userid);

                await _context.Admins.AddAsync(newAdmin);
                await _context.SaveChangesAsync();

                return Ok(newAdmin);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [HttpPut("update-admin/{id}")]
        public async Task<IActionResult> UpdateInforAdmin([FromForm] UpdateAdminDto updateAdminDto, int id)
        {
            try
            {
                var admin = await _adminService.UpdateInforAdmin(updateAdminDto, id);

                await _context.SaveChangesAsync();

                return Ok(admin);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}