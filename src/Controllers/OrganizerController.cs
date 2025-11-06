using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Services.Interfaces;
using System.Security.Claims;
using StudentActivities.src.Dtos.Events;
using StudentActivities.src.Dtos.Clubs;
using StudentActivities.src.Dtos.Organizers;
using StudentActivities.src.Data;
using System;


namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/organizer")]
    public class OrganizerController : ControllerBase
    {
        private readonly IEventService _eventSvc;
        private readonly IClubService _clubSvc;
        private readonly IOrganizerService _organizer;

        private readonly ApplicationDbContext _context;

        public OrganizerController(IEventService eventSvc, IClubService clubSvc,
    IOrganizerService organizerService, ApplicationDbContext context)
        {
            _eventSvc = eventSvc;
            _clubSvc = clubSvc;
            _organizer = organizerService;
            _context = context;
        }

        private IActionResult ReturnException(Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }

        private int GetOrganizerId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : throw new UnauthorizedAccessException();
        }

        // 1. XEM DANH SÁCH
        [HttpGet("my-items")]
        public async Task<IActionResult> GetMyItems()
        {
            var organizerId = GetOrganizerId();

            var events = (await _eventSvc.GetAllAsync())
                .Where(e => e.OrganizerId == organizerId)
                .ToList();

            var clubs = (await _clubSvc.GetAllAsync())
                .Where(c => c.OrganizerId == organizerId)
                .ToList();

            return Ok(new { Events = events, Clubs = clubs });
        }

        // 2. CHỈNH SỬA
        [HttpPut("events/{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDto dto)
        {
            var organizerId = GetOrganizerId();
            var ev = await _eventSvc.GetByIdAsync(id);
            if (ev == null || ev.OrganizerId != organizerId) return Forbid();

            var result = await _eventSvc.UpdateAsync(id, dto);
            return result ? NoContent() : NotFound();
        }

        [HttpPut("clubs/{id}")]
        public async Task<IActionResult> UpdateClub(int id, [FromBody] UpdateClubDto dto)
        {
            var organizerId = GetOrganizerId();
            var club = await _clubSvc.GetByIdAsync(id);
            if (club == null || club.OrganizerId != organizerId) return Forbid();

            var result = await _clubSvc.UpdateAsync(id, dto);
            return result ? NoContent() : NotFound();
        }

        // 3. XÓA
        [HttpDelete("events/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var organizerId = GetOrganizerId();
            var ev = await _eventSvc.GetByIdAsync(id);
            if (ev == null || ev.OrganizerId != organizerId) return Forbid();

            var result = await _eventSvc.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("clubs/{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var organizerId = GetOrganizerId();
            var club = await _clubSvc.GetByIdAsync(id);
            if (club == null || club.OrganizerId != organizerId) return Forbid();

            var result = await _clubSvc.DeleteAsync(id);
            return result ? NoContent() : NotFound();
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
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("create-organizer")]
        public async Task<IActionResult> CreateOrganizer([FromForm] CreateOrganizerDto createOrganizerDto, int userid)
        {
            try
            {
                var newOrganizer = await _organizer.CreateOrganizer(createOrganizerDto, userid);
                return Ok(newOrganizer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("update-organizer/{id}")]
        public async Task<IActionResult> UpdateInforOrganizer([FromForm] UpdateOrganizerDto updateOrganizerDto, int id)
        {
            try
            {
                var organizer = await _organizer.UpdateInforOrganizer(updateOrganizerDto, id);
                return Ok(organizer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}