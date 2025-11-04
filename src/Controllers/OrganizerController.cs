using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Organizers;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizerController : ApiControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrganizerService _organizer;

        public OrganizerController(ApplicationDbContext context, IOrganizerService organizer, ILogger<OrganizerController> logger) : base(logger)
        {
            _context = context;
            _organizer = organizer;
        }

        [HttpGet("get-all-organizer")]
        public async Task<IActionResult> GetAllOrganizer()
        {
            try
            {
                var organizer = await _organizer.GetAllOrganizer();

                return Ok(organizer);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [HttpPost("create-organizer")]
        public async Task<IActionResult> CreateOrganizer([FromForm] CreateOrganizerDto createOrganizerDto, int userid)
        {
            try
            {
                var newOrganizer = await _organizer.CreateOrganizer(createOrganizerDto, userid);

                await _context.Organizers.AddAsync(newOrganizer);
                await _context.SaveChangesAsync();

                return Ok(newOrganizer);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [HttpPut("update-organizer/{id}")]
        public async Task<IActionResult> UpdateInforOrganizer([FromForm] UpdateOrganizerDto updateOrganizerDto, int id)
        {
            try
            {
                var organizer = await _organizer.UpdateInforOrganizer(updateOrganizerDto, id);

                await _context.SaveChangesAsync();

                return Ok(organizer);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}