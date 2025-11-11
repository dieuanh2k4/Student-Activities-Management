using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Events;
using StudentActivities.src.Services.Interfaces;
using StudentActivities.src.Services.Extensions;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ApiControllerBase
    {
        private readonly IEventService _svc;
        private readonly INotificationService _notificationService;

        public EventsController(IEventService svc, INotificationService notificationService, ILogger<EventsController> logger) : base(logger)
        {
            _svc = svc;
            _notificationService = notificationService;
        }

        [Authorize(Roles = "Admin,Organizer")]  // Chỉ Admin và Organizer tạo event
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateEventDto dto, IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var imageUrl = await _svc.UploadImageAsync(file);
                    dto.Thumbnail = imageUrl;
                }

                var created = await _svc.CreateAsync(dto);

                // Tự động gửi thông báo khi tạo sự kiện mới
                if (created.Id > 0 && !string.IsNullOrEmpty(created.Name))
                {
                    await _notificationService.SendEventNotificationAsync(
                        created.Id,
                        created.Name,
                        NotificationType.EventCreated);
                }

                return Ok(created);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [AllowAnonymous]  // Public: Ai cũng xem được danh sách events
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _svc.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [AllowAnonymous]  // Public: Ai cũng xem được chi tiết event
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _svc.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [Authorize(Roles = "Admin,Organizer")]  // Chỉ Admin và Organizer sửa event
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEventDto dto)
        {
            try
            {
                var existingEvent = await _svc.GetByIdAsync(id);
                if (existingEvent == null) return NotFound();

                var ok = await _svc.UpdateAsync(id, dto);
                if (!ok) return NotFound();

                // Gửi thông báo cập nhật sự kiện
                await _notificationService.SendEventNotificationAsync(
                    id,
                    existingEvent.Name ?? "Sự kiện",
                    NotificationType.EventUpdated);

                return NoContent();
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [Authorize(Roles = "Admin")]  // Chỉ Admin xóa event
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingEvent = await _svc.GetByIdAsync(id);
                if (existingEvent == null) return NotFound();

                var ok = await _svc.DeleteAsync(id);
                if (!ok) return NotFound();

                // Gửi thông báo hủy sự kiện
                await _notificationService.SendEventNotificationAsync(
                    id,
                    existingEvent.Name ?? "Sự kiện",
                    NotificationType.EventCancelled);

                return NoContent();
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Gửi thông báo nhắc nhở cho sự kiện cụ thể
        /// </summary>
        [HttpPost("{id:int}/send-reminder")]
        public async Task<IActionResult> SendEventReminder(int id)
        {
            try
            {
                var eventItem = await _svc.GetByIdAsync(id);
                if (eventItem == null) return NotFound();

                await _notificationService.SendEventNotificationAsync(
                    id,
                    eventItem.Name ?? "Sự kiện",
                    NotificationType.EventReminder);

                return Ok(new { message = "Đã gửi thông báo nhắc nhở thành công" });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}