using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Notifications;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationDto>> CreateNotificationAsync(CreateNotificationDto dto)
        {
            var notifications = new List<Notifications>();
            var studentIds = new List<int>();

            // Xác định danh sách sinh viên nhận thông báo
            if (dto.SendToAllStudents)
            {
                // Gửi cho tất cả sinh viên
                studentIds = await _context.Students.Select(s => s.Id).ToListAsync();
            }
            else if (dto.SendToEventRegistered && dto.EventId.HasValue)
            {
                // Gửi cho sinh viên đã đăng ký sự kiện
                studentIds = await _context.Resgistrations
                    .Where(r => r.EventId == dto.EventId.Value)
                    .Select(r => r.StudentId)
                    .ToListAsync();
            }
            else if (dto.SendToClubMembers && dto.ClubId.HasValue)
            {
                // Gửi cho thành viên câu lạc bộ
                studentIds = await _context.Resgistrations
                    .Where(r => r.ClubId == dto.ClubId.Value)
                    .Select(r => r.StudentId)
                    .Distinct()
                    .ToListAsync();
            }
            else if (dto.StudentIds != null && dto.StudentIds.Any())
            {
                // Gửi cho danh sách sinh viên cụ thể
                studentIds = dto.StudentIds;
            }

            // Tạo thông báo cho từng sinh viên
            foreach (var studentId in studentIds)
            {
                var notification = NotificationMapper.ToEntity(dto, studentId);
                notifications.Add(notification);
            }

            if (notifications.Any())
            {
                _context.Notifications.AddRange(notifications);
                await _context.SaveChangesAsync();
            }

            // Trả về danh sách thông báo đã tạo
            return notifications.Select(NotificationMapper.ToDto).ToList();
        }

        public async Task<List<NotificationDto>> GetNotificationsByStudentIdAsync(int studentId)
        {
            var notifications = await _context.Notifications
                .Include(n => n.Events)
                .Include(n => n.Clubs)
                .Include(n => n.Students)
                .Where(n => n.StudentId == studentId)
                .OrderByDescending(n => n.SendDate)
                .AsNoTracking()
                .ToListAsync();

            return notifications.Select(NotificationMapper.ToDto).ToList();
        }

        public async Task<List<NotificationDto>> GetAllNotificationsAsync()
        {
            var notifications = await _context.Notifications
                .Include(n => n.Events)
                .Include(n => n.Clubs)
                .Include(n => n.Students)
                .OrderByDescending(n => n.SendDate)
                .AsNoTracking()
                .ToListAsync();

            return notifications.Select(NotificationMapper.ToDto).ToList();
        }

        public async Task<NotificationDto?> GetNotificationByIdAsync(int id)
        {
            var notification = await _context.Notifications
                .Include(n => n.Events)
                .Include(n => n.Clubs)
                .Include(n => n.Students)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id);

            return notification == null ? null : NotificationMapper.ToDto(notification);
        }

        public async Task<bool> UpdateNotificationStatusAsync(int id, UpdateNotificationStatusDto dto)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if (notification == null) return false;

            notification.Status = dto.Status;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if (notification == null) return false;

            notification.Status = "Đã đọc";
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsUnreadAsync(int id)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if (notification == null) return false;

            notification.Status = "Chưa đọc";
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<NotificationSummaryDto> GetNotificationSummaryByStudentIdAsync(int studentId)
        {
            var notifications = await _context.Notifications
                .Include(n => n.Events)
                .Include(n => n.Clubs)
                .Include(n => n.Students)
                .Where(n => n.StudentId == studentId)
                .OrderByDescending(n => n.SendDate)
                .AsNoTracking()
                .ToListAsync();

            var total = notifications.Count;
            var unread = notifications.Count(n => n.Status == "Chưa đọc");
            var read = notifications.Count(n => n.Status == "Đã đọc");
            var recent = notifications.Take(10).Select(NotificationMapper.ToDto).ToList();

            return new NotificationSummaryDto
            {
                TotalNotifications = total,
                UnreadNotifications = unread,
                ReadNotifications = read,
                RecentNotifications = recent
            };
        }

        public async Task<List<NotificationDto>> GetNotificationsByEventIdAsync(int eventId)
        {
            var notifications = await _context.Notifications
                .Include(n => n.Events)
                .Include(n => n.Clubs)
                .Include(n => n.Students)
                .Where(n => n.EventId == eventId)
                .OrderByDescending(n => n.SendDate)
                .AsNoTracking()
                .ToListAsync();

            return notifications.Select(NotificationMapper.ToDto).ToList();
        }

        public async Task<List<NotificationDto>> GetNotificationsByClubIdAsync(int clubId)
        {
            var notifications = await _context.Notifications
                .Include(n => n.Events)
                .Include(n => n.Clubs)
                .Include(n => n.Students)
                .Where(n => n.ClubId == clubId)
                .OrderByDescending(n => n.SendDate)
                .AsNoTracking()
                .ToListAsync();

            return notifications.Select(NotificationMapper.ToDto).ToList();
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUnreadCountByStudentIdAsync(int studentId)
        {
            return await _context.Notifications
                .Where(n => n.StudentId == studentId && n.Status == "Chưa đọc")
                .CountAsync();
        }

        public async Task<PagedNotificationDto> GetNotificationsWithFilterAsync(NotificationFilterDto filter)
        {
            var query = _context.Notifications
                .Include(n => n.Events)
                .Include(n => n.Clubs)
                .Include(n => n.Students)
                .AsQueryable();

            // Áp dụng các bộ lọc
            if (filter.StudentId.HasValue)
                query = query.Where(n => n.StudentId == filter.StudentId.Value);

            if (filter.EventId.HasValue)
                query = query.Where(n => n.EventId == filter.EventId.Value);

            if (filter.ClubId.HasValue)
                query = query.Where(n => n.ClubId == filter.ClubId.Value);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(n => n.Status == filter.Status);

            if (filter.FromDate.HasValue)
                query = query.Where(n => n.SendDate >= DateOnly.FromDateTime(filter.FromDate.Value));

            if (filter.ToDate.HasValue)
                query = query.Where(n => n.SendDate <= DateOnly.FromDateTime(filter.ToDate.Value));

            // Sắp xếp
            if (filter.SortBy.ToLower() == "senddate")
            {
                query = filter.SortOrder.ToLower() == "asc"
                    ? query.OrderBy(n => n.SendDate)
                    : query.OrderByDescending(n => n.SendDate);
            }
            else if (filter.SortBy.ToLower() == "status")
            {
                query = filter.SortOrder.ToLower() == "asc"
                    ? query.OrderBy(n => n.Status)
                    : query.OrderByDescending(n => n.Status);
            }
            else
            {
                query = query.OrderByDescending(n => n.SendDate);
            }

            // Đếm tổng số bản ghi
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);

            // Phân trang
            var notifications = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedNotificationDto
            {
                Notifications = notifications.Select(NotificationMapper.ToDto).ToList(),
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                HasNextPage = filter.Page < totalPages,
                HasPreviousPage = filter.Page > 1
            };
        }

        public async Task<bool> MarkAllAsReadAsync(int studentId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.StudentId == studentId && n.Status == "Chưa đọc")
                .ToListAsync();

            if (!notifications.Any()) return false;

            foreach (var notification in notifications)
            {
                notification.Status = "Đã đọc";
            }

            _context.Notifications.UpdateRange(notifications);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllNotificationsAsync(int studentId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.StudentId == studentId)
                .ToListAsync();

            if (!notifications.Any()) return false;

            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}