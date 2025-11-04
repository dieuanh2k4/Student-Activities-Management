using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Clubs;
using StudentActivities.src.Dtos.Events;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/organizers")]
    [Authorize(Roles = "Organizer")]
    public class OrganizersController : ControllerBase
    {
        private readonly IOrganizerService _svc;

        public OrganizersController(IOrganizerService svc) => _svc = svc;

        // 1. Xem dashboard
        [HttpGet("{organizerId}/dashboard")]
        public async Task<IActionResult> GetDashboard(int organizerId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value!);
            if (userId != organizerId) return Forbid();

            var result = await _svc.GetDashboardAsync(organizerId);
            return Ok(result);
        }

        // 2. Xóa sự kiện
        [HttpDelete("{organizerId}/events/{eventId}")]
        public async Task<IActionResult> DeleteEvent(int organizerId, int eventId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value!);
            if (userId != organizerId) return Forbid();

            var success = await _svc.DeleteEventAsync(organizerId, eventId);
            return success ? NoContent() : NotFound();
        }

        // 3. Cập nhật sự kiện
        [HttpPut("{organizerId}/events/{eventId}")]
        public async Task<IActionResult> UpdateEvent(int organizerId, int eventId, [FromBody] UpdateEventDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value!);
            if (userId != organizerId) return Forbid();

            var success = await _svc.UpdateEventAsync(organizerId, eventId, dto);
            return success ? NoContent() : NotFound();
        }

        // 4. Cập nhật câu lạc bộ
        [HttpPut("{organizerId}/clubs/{clubId}")]
        public async Task<IActionResult> UpdateClub(int organizerId, int clubId, [FromBody] UpdateClubDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value!);
            if (userId != organizerId) return Forbid();

            var success = await _svc.UpdateClubAsync(organizerId, clubId, dto);
            return success ? NoContent() : NotFound();
        }
    }
}