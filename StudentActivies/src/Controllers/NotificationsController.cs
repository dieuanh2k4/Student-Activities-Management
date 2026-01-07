using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Notifications;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class NotificationsController : ApiControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService, ILogger<NotificationsController> logger)
            : base(logger)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Tạo thông báo mới - Chỉ Admin và Organizer
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
        {
            try
            {
                var notifications = await _notificationService.CreateNotificationAsync(dto);
                return Ok(new
                {
                    message = $"Đã tạo {notifications.Count} thông báo thành công",
                    notifications = notifications
                });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy tất cả thông báo - Chỉ Admin
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var notifications = await _notificationService.GetAllNotificationsAsync();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy thông báo của sinh viên cụ thể
        /// </summary>
        [HttpGet("student/{studentId:int}")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> GetNotificationsByStudentId(int studentId)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationsByStudentIdAsync(studentId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy tóm tắt thông báo của sinh viên
        /// </summary>
        [HttpGet("student/{studentId:int}/summary")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> GetNotificationSummaryByStudentId(int studentId)
        {
            try
            {
                var summary = await _notificationService.GetNotificationSummaryByStudentIdAsync(studentId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy số lượng thông báo chưa đọc của sinh viên
        /// </summary>
        [HttpGet("student/{studentId:int}/unread-count")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> GetUnreadCountByStudentId(int studentId)
        {
            try
            {
                var count = await _notificationService.GetUnreadCountByStudentIdAsync(studentId);
                return Ok(new { unreadCount = count });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy thông báo theo sự kiện
        /// </summary>
        [HttpGet("event/{eventId:int}")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetNotificationsByEventId(int eventId)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationsByEventIdAsync(eventId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy thông báo theo câu lạc bộ
        /// </summary>
        [HttpGet("club/{clubId:int}")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetNotificationsByClubId(int clubId)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationsByClubIdAsync(clubId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy thông báo theo ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            try
            {
                var notification = await _notificationService.GetNotificationByIdAsync(id);
                if (notification == null) return NotFound();
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái thông báo
        /// </summary>
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateNotificationStatus(int id, [FromBody] UpdateNotificationStatusDto dto)
        {
            try
            {
                var success = await _notificationService.UpdateNotificationStatusAsync(id, dto);
                if (!success) return NotFound();
                return Ok(new { message = "Đã cập nhật trạng thái thông báo thành công" });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Đánh dấu thông báo là đã đọc
        /// </summary>
        [HttpPut("{id:int}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var success = await _notificationService.MarkAsReadAsync(id);
                if (!success) return NotFound();
                return Ok(new { message = "Đã đánh dấu thông báo là đã đọc" });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Đánh dấu thông báo là chưa đọc
        /// </summary>
        [HttpPut("{id:int}/mark-unread")]
        public async Task<IActionResult> MarkAsUnread(int id)
        {
            try
            {
                var success = await _notificationService.MarkAsUnreadAsync(id);
                if (!success) return NotFound();
                return Ok(new { message = "Đã đánh dấu thông báo là chưa đọc" });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Xóa thông báo - Chỉ Admin
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var success = await _notificationService.DeleteNotificationAsync(id);
                if (!success) return NotFound();
                return Ok(new { message = "Đã xóa thông báo thành công" });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Lấy thông báo với bộ lọc và phân trang
        /// </summary>
        [HttpPost("filter")]
        public async Task<IActionResult> GetNotificationsWithFilter([FromBody] NotificationFilterDto filter)
        {
            try
            {
                var result = await _notificationService.GetNotificationsWithFilterAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Đánh dấu tất cả thông báo của sinh viên là đã đọc
        /// </summary>
        [HttpPut("student/{studentId:int}/mark-all-read")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> MarkAllAsRead(int studentId)
        {
            try
            {
                var success = await _notificationService.MarkAllAsReadAsync(studentId);
                if (!success) return NotFound("Không có thông báo chưa đọc nào");
                return Ok(new { message = "Đã đánh dấu tất cả thông báo là đã đọc" });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        /// <summary>
        /// Xóa tất cả thông báo của sinh viên - Chỉ Admin hoặc chính sinh viên đó
        /// </summary>
        [HttpDelete("student/{studentId:int}/all")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> DeleteAllNotifications(int studentId)
        {
            try
            {
                var success = await _notificationService.DeleteAllNotificationsAsync(studentId);
                if (!success) return NotFound("Không có thông báo nào để xóa");
                return Ok(new { message = "Đã xóa tất cả thông báo thành công" });
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}