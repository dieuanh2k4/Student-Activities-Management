using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Users;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public UserController(ApplicationDbContext context, IUserService userService, ILogger<UserController> logger) : base(logger)
        {
            _context = context;
            _userService = userService;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-user")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var user = await _userService.GetAllUsers();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        // [AllowAnonymous]  // Cho phép tạo user không cần đăng nhập
        public async Task<IActionResult> CreateUser([FromForm] CreateUserDto createUserDto)
        {
            try
            {
                var newUser = await _userService.CreateUser(createUserDto);

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto updateUserDto, int id)
        {
            try
            {
                var updateUser = await _userService.UpdateUser(updateUserDto, id);

                await _context.SaveChangesAsync();

                return Ok(updateUser);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userService.DeleteUser(id);

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(_userService);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}