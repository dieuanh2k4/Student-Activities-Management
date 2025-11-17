using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Checkins;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class CheckinService : ICheckinService
    {
        private readonly ApplicationDbContext _context;

        public CheckinService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Chức năng 1: Lấy danh sách sinh viên đã đăng ký sự kiện
        /// </summary>
        public async Task<List<CheckinDto>> GetEventRegistrationsAsync(int eventId)
        {
            // Kiểm tra sự kiện có tồn tại không
            var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
            if (!eventExists)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found");
            }

            // Lấy danh sách đăng ký
            var registrations = await _context.Resgistrations
                .Include(r => r.Students)
                    .ThenInclude(s => s!.Users)
                .Include(r => r.Students)
                    .ThenInclude(s => s!.AcademicClasses)
                .Include(r => r.Events)
                .Where(r => r.EventId == eventId)
                .ToListAsync();

            // Lấy thông tin check-in nếu đã có
            var checkins = await _context.Checkins
                .Include(c => c.Students)
                    .ThenInclude(s => s!.Users)
                .Include(c => c.Students)
                    .ThenInclude(s => s!.AcademicClasses)
                .Include(c => c.Events)
                .Include(c => c.CheckedInByUser)
                    .ThenInclude(u => u!.Admins)
                .Include(c => c.CheckedInByUser)
                    .ThenInclude(u => u!.Organizers)
                .Where(c => c.EventId == eventId)
                .ToListAsync();

            var result = new List<CheckinDto>();

            foreach (var registration in registrations)
            {
                var checkin = checkins.FirstOrDefault(c => c.StudentId == registration.StudentId);

                if (checkin != null)
                {
                    // Đã có record check-in
                    result.Add(checkin.ToCheckinDto());
                }
                else
                {
                    // Chưa có record check-in, tạo DTO từ registration
                    result.Add(registration.ToCheckinDtoFromRegistration(eventId));
                }
            }

            return result.OrderBy(c => c.StudentCode).ToList();
        }

        /// <summary>
        /// Chức năng 2: Xem trạng thái check-in của sinh viên trong sự kiện
        /// </summary>
        public async Task<List<CheckinDto>> GetEventCheckinsAsync(int eventId)
        {
            // Kiểm tra sự kiện có tồn tại không
            var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
            if (!eventExists)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found");
            }

            // Lấy danh sách check-in
            var checkins = await _context.Checkins
                .Include(c => c.Students)
                    .ThenInclude(s => s!.Users)
                .Include(c => c.Students)
                    .ThenInclude(s => s!.AcademicClasses)
                .Include(c => c.Events)
                .Include(c => c.CheckedInByUser)
                    .ThenInclude(u => u!.Admins)
                .Include(c => c.CheckedInByUser)
                    .ThenInclude(u => u!.Organizers)
                .Where(c => c.EventId == eventId)
                .ToListAsync();

            return checkins.Select(c => c.ToCheckinDto()).OrderBy(c => c.StudentCode).ToList();
        }

        /// <summary>
        /// Chức năng 3: Cập nhật trạng thái check-in (Manual)
        /// </summary>
        public async Task<CheckinDto> UpdateCheckinStatusAsync(int eventId, int studentId, UpdateCheckinStatusDto dto, int userId)
        {
            // Kiểm tra sự kiện có tồn tại không
            var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
            if (!eventExists)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found");
            }

            // Kiểm tra sinh viên có đăng ký sự kiện không
            var registration = await _context.Resgistrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.StudentId == studentId);

            if (registration == null)
            {
                throw new InvalidOperationException($"Student {studentId} has not registered for Event {eventId}");
            }

            // Tìm hoặc tạo mới record check-in
            var checkin = await _context.Checkins
                .FirstOrDefaultAsync(c => c.EventId == eventId && c.StudentId == studentId);

            if (checkin == null)
            {
                // Tạo mới record check-in
                checkin = new Checkin
                {
                    EventId = eventId,
                    StudentId = studentId,
                    Status = dto.Status,
                    CheckInTime = dto.Status == "Đã tham dự" ? DateTime.Now : null,
                    CheckedInBy = dto.Status == "Đã tham dự" ? userId : null
                };

                _context.Checkins.Add(checkin);
            }
            else
            {
                // Cập nhật record có sẵn
                checkin.Status = dto.Status;
                checkin.CheckInTime = dto.Status == "Đã tham dự" ? (checkin.CheckInTime ?? DateTime.Now) : null;
                checkin.CheckedInBy = dto.Status == "Đã tham dự" ? userId : null;
            }

            await _context.SaveChangesAsync();

            // Load lại với đầy đủ thông tin
            checkin = await _context.Checkins
                .Include(c => c.Students)
                    .ThenInclude(s => s!.Users)
                .Include(c => c.Students)
                    .ThenInclude(s => s!.AcademicClasses)
                .Include(c => c.Events)
                .Include(c => c.CheckedInByUser)
                    .ThenInclude(u => u!.Admins)
                .Include(c => c.CheckedInByUser)
                    .ThenInclude(u => u!.Organizers)
                .FirstOrDefaultAsync(c => c.Id == checkin.Id);

            return checkin!.ToCheckinDto();
        }

        /// <summary>
        /// Tìm kiếm sinh viên trong sự kiện (hỗ trợ cho check-in thủ công)
        /// </summary>
        public async Task<List<CheckinDto>> SearchStudentsInEventAsync(int eventId, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return await GetEventRegistrationsAsync(eventId);
            }

            var normalizedQuery = query.ToLower().Trim();

            // Lấy danh sách đăng ký
            var registrations = await _context.Resgistrations
                .Include(r => r.Students)
                    .ThenInclude(s => s!.Users)
                .Include(r => r.Students)
                    .ThenInclude(s => s!.AcademicClasses)
                .Include(r => r.Events)
                .Where(r => r.EventId == eventId)
                .Where(r =>
                    r.Students!.Users!.UserName!.ToLower().Contains(normalizedQuery) ||
                    r.Students.FirstName!.ToLower().Contains(normalizedQuery) ||
                    r.Students.LastName!.ToLower().Contains(normalizedQuery) ||
                    (r.Students.FirstName + " " + r.Students.LastName).ToLower().Contains(normalizedQuery) ||
                    r.Students.Email!.ToLower().Contains(normalizedQuery)
                )
                .ToListAsync();

            // Lấy thông tin check-in
            var studentIds = registrations.Select(r => r.StudentId).ToList();
            var checkins = await _context.Checkins
                .Include(c => c.Students)
                    .ThenInclude(s => s!.Users)
                .Include(c => c.Students)
                    .ThenInclude(s => s!.AcademicClasses)
                .Include(c => c.Events)
                .Include(c => c.CheckedInByUser)
                    .ThenInclude(u => u!.Admins)
                .Include(c => c.CheckedInByUser)
                    .ThenInclude(u => u!.Organizers)
                .Where(c => c.EventId == eventId && studentIds.Contains(c.StudentId))
                .ToListAsync();

            var result = new List<CheckinDto>();

            foreach (var registration in registrations)
            {
                var checkin = checkins.FirstOrDefault(c => c.StudentId == registration.StudentId);

                if (checkin != null)
                {
                    result.Add(checkin.ToCheckinDto());
                }
                else
                {
                    result.Add(registration.ToCheckinDtoFromRegistration(eventId));
                }
            }

            return result.OrderBy(c => c.StudentCode).ToList();
        }

        /// <summary>
        /// Lấy thống kê check-in của sự kiện
        /// </summary>
        public async Task<CheckinStatisticsDto> GetCheckinStatisticsAsync(int eventId)
        {
            var eventInfo = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventInfo == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found");
            }

            var totalRegistrations = await _context.Resgistrations
                .CountAsync(r => r.EventId == eventId);

            var checkins = await _context.Checkins
                .Where(c => c.EventId == eventId)
                .ToListAsync();

            var totalAttended = checkins.Count(c => c.Status == "Đã tham dự");
            var totalAbsent = checkins.Count(c => c.Status == "Vắng mặt");

            // Sinh viên chưa có record check-in = chưa điểm danh
            var notCheckedIn = totalRegistrations - checkins.Count;
            totalAbsent += notCheckedIn;

            var attendanceRate = totalRegistrations > 0
                ? Math.Round((double)totalAttended / totalRegistrations * 100, 2)
                : 0;

            var lastUpdated = checkins.Any()
                ? checkins.Max(c => c.CheckInTime)
                : null;

            return new CheckinStatisticsDto
            {
                EventId = eventId,
                EventName = eventInfo.Name,
                TotalRegistrations = totalRegistrations,
                TotalAttended = totalAttended,
                TotalAbsent = totalAbsent,
                AttendanceRate = attendanceRate,
                LastUpdated = lastUpdated
            };
        }

        /// <summary>
        /// Check-in hàng loạt (Admin only)
        /// </summary>
        public async Task<List<CheckinDto>> BulkCheckinAsync(int eventId, List<int> studentIds, int userId)
        {
            var result = new List<CheckinDto>();

            foreach (var studentId in studentIds)
            {
                try
                {
                    var dto = new UpdateCheckinStatusDto { Status = "Đã tham dự" };
                    var checkinDto = await UpdateCheckinStatusAsync(eventId, studentId, dto, userId);
                    result.Add(checkinDto);
                }
                catch (Exception)
                {
                    // Skip students that can't be checked in
                    continue;
                }
            }

            return result;
        }
    }
}
