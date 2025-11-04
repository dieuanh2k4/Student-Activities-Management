using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Resgistrations;
using StudentActivities.src.Exceptions;
using StudentActivities.src.Mappers;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class ResgistrationService : IResgistrationService
    {
        private readonly ApplicationDbContext _context;

        public ResgistrationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResgistrationResult<List<AvailableActivityDto>>> GetAvailableActivitiesAsync(int studentId, string? type = null)
        {
            var now = DateTime.Now;
            var result = new List<AvailableActivityDto>();

            // Lấy danh sách đã đăng ký của sinh viên
            var registeredIds = await _context.Resgistrations
                .Where(r => r.StudentId == studentId && r.Status != "CANCELLED")
                .Select(r => new { r.EventId, r.ClubId, r.Type })
                .ToListAsync();

            // Nếu không filter type hoặc type = EVENT
            if (string.IsNullOrEmpty(type) || type.ToUpper() == "EVENT")
            {
                var events = await _context.Events
                    .Where(e => e.RegistrationEndDate >= now)
                    .ToListAsync();

                foreach (var evt in events)
                {
                    var isRegistered = registeredIds.Any(r => r.EventId == evt.Id && r.Type == "EVENT");
                    result.Add(evt.ToAvailableActivityDto(isRegistered));
                }
            }

            // Nếu không filter type hoặc type = CLUB
            if (string.IsNullOrEmpty(type) || type.ToUpper() == "CLUB")
            {
                var clubs = await _context.Clubs
                    .Where(c => c.RegistrationEndDate >= now)
                    .ToListAsync();

                foreach (var club in clubs)
                {
                    var isRegistered = registeredIds.Any(r => r.ClubId == club.Id && r.Type == "CLUB");
                    result.Add(club.ToAvailableActivityDto(isRegistered));
                }
            }

            // Sắp xếp theo RegistrationEndDate
            result = result.OrderBy(x => x.RegistrationEndDate).ToList();

            return ResgistrationResult<List<AvailableActivityDto>>.Success(result);
        }

        public async Task<ResgistrationResult<ResgistrationDto>> RegisterActivityAsync(int studentId, CreateResgistrationDto dto)
        {
            var now = DateTime.Now;

            // Validate input
            if (dto.Type.ToUpper() == "EVENT" && !dto.EventId.HasValue)
                return ResgistrationResult<ResgistrationDto>.Failure("EventId là bắt buộc khi đăng ký sự kiện");

            if (dto.Type.ToUpper() == "CLUB" && !dto.ClubId.HasValue)
                return ResgistrationResult<ResgistrationDto>.Failure("ClubId là bắt buộc khi đăng ký CLB");

            // Kiểm tra đã đăng ký chưa
            var existingRegistration = await _context.Resgistrations
                .FirstOrDefaultAsync(r => 
                    r.StudentId == studentId && 
                    r.Type == dto.Type.ToUpper() &&
                    (dto.Type.ToUpper() == "EVENT" ? r.EventId == dto.EventId : r.ClubId == dto.ClubId) &&
                    r.Status != "CANCELLED");

            if (existingRegistration != null)
                return ResgistrationResult<ResgistrationDto>.Failure("Bạn đã đăng ký hoạt động này rồi");

            if (dto.Type.ToUpper() == "EVENT")
            {
                return await RegisterEventAsync(studentId, dto.EventId!.Value, now);
            }
            else if (dto.Type.ToUpper() == "CLUB")
            {
                return await RegisterClubAsync(studentId, dto.ClubId!.Value, now);
            }

            return ResgistrationResult<ResgistrationDto>.Failure("Type không hợp lệ. Chỉ chấp nhận EVENT hoặc CLUB");
        }

        private async Task<ResgistrationResult<ResgistrationDto>> RegisterEventAsync(int studentId, int eventId, DateTime now)
        {
            var eventItem = await _context.Events.FindAsync(eventId);
            if (eventItem == null)
                return ResgistrationResult<ResgistrationDto>.Failure("Sự kiện không tồn tại");

            // Kiểm tra thời gian
            if (now > eventItem.RegistrationEndDate)
                return ResgistrationResult<ResgistrationDto>.Failure("Đã hết hạn đăng ký");

            // Kiểm tra số lượng
            if (eventItem.CurrentRegistrations >= eventItem.MaxCapacity)
                return ResgistrationResult<ResgistrationDto>.Failure("Sự kiện đã đầy");

            // Tạo đăng ký
            var registration = new Resgistrations
            {
                StudentId = studentId,
                EventId = eventId,
                Type = "EVENT",
                Status = "REGISTERED",
                RegistrationDate = now
            };

            _context.Resgistrations.Add(registration);

            // Tăng số lượng đăng ký
            eventItem.CurrentRegistrations++;

            await _context.SaveChangesAsync();

            // Load navigation properties
            await _context.Entry(registration)
                .Reference(r => r.Students)
                .LoadAsync();
            await _context.Entry(registration)
                .Reference(r => r.Events)
                .LoadAsync();

            return ResgistrationResult<ResgistrationDto>.Success(registration.ToDto());
        }

        private async Task<ResgistrationResult<ResgistrationDto>> RegisterClubAsync(int studentId, int clubId, DateTime now)
        {
            var club = await _context.Clubs.FindAsync(clubId);
            if (club == null)
                return ResgistrationResult<ResgistrationDto>.Failure("CLB không tồn tại");

            // Kiểm tra thời gian
            if (now > club.RegistrationEndDate)
                return ResgistrationResult<ResgistrationDto>.Failure("Đã hết hạn đăng ký");

            // Kiểm tra số lượng
            if (club.CurrentRegistrations >= club.MaxCapacity)
                return ResgistrationResult<ResgistrationDto>.Failure("CLB đã đầy");

            // Tạo đăng ký
            var registration = new Resgistrations
            {
                StudentId = studentId,
                ClubId = clubId,
                Type = "CLUB",
                Status = "REGISTERED",
                RegistrationDate = now
            };

            _context.Resgistrations.Add(registration);

            // Tăng số lượng đăng ký
            club.CurrentRegistrations++;

            await _context.SaveChangesAsync();

            // Load navigation properties
            await _context.Entry(registration)
                .Reference(r => r.Students)
                .LoadAsync();
            await _context.Entry(registration)
                .Reference(r => r.Clubs)
                .LoadAsync();

            return ResgistrationResult<ResgistrationDto>.Success(registration.ToDto());
        }

        public async Task<ResgistrationResult<bool>> CancelRegistrationAsync(int studentId, int activityId, string type)
        {
            var now = DateTime.Now;

            var registration = await _context.Resgistrations
                .Include(r => r.Events)
                .Include(r => r.Clubs)
                .FirstOrDefaultAsync(r => 
                    r.StudentId == studentId && 
                    r.Type == type.ToUpper() &&
                    (type.ToUpper() == "EVENT" ? r.EventId == activityId : r.ClubId == activityId) &&
                    r.Status == "REGISTERED");

            if (registration == null)
                return ResgistrationResult<bool>.Failure("Không tìm thấy đăng ký hoặc bạn đã hủy rồi");

            // Kiểm tra thời gian hủy
            DateTime? registrationEndDate = null;
            if (type.ToUpper() == "EVENT" && registration.Events != null)
            {
                registrationEndDate = registration.Events.RegistrationEndDate;
            }
            else if (type.ToUpper() == "CLUB" && registration.Clubs != null)
            {
                registrationEndDate = registration.Clubs.RegistrationEndDate;
            }

            if (registrationEndDate.HasValue && now > registrationEndDate.Value)
                return ResgistrationResult<bool>.Failure("Đã quá hạn hủy đăng ký");

            // Cập nhật trạng thái
            registration.Status = "CANCELLED";
            registration.CancellationTime = now;

            // Giảm số lượng đăng ký
            if (type.ToUpper() == "EVENT" && registration.Events != null)
            {
                registration.Events.CurrentRegistrations--;
            }
            else if (type.ToUpper() == "CLUB" && registration.Clubs != null)
            {
                registration.Clubs.CurrentRegistrations--;
            }

            await _context.SaveChangesAsync();

            return ResgistrationResult<bool>.Success(true);
        }

        public async Task<ResgistrationResult<List<ResgistrationDto>>> GetMyRegistrationsAsync(int studentId, string? type = null, string? status = null)
        {
            var query = _context.Resgistrations
                .Include(r => r.Students)
                .Include(r => r.Events)
                .Include(r => r.Clubs)
                .Where(r => r.StudentId == studentId);

            // Filter by type
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(r => r.Type == type.ToUpper());
            }

            // Filter by status
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.Status == status.ToUpper());
            }

            var registrations = await query
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();

            var result = registrations.Select(r => r.ToDto()).ToList();

            return ResgistrationResult<List<ResgistrationDto>>.Success(result);
        }

        public async Task<ResgistrationResult<ResgistrationDto>> GetRegistrationDetailAsync(int studentId, int activityId, string type)
        {
            var registration = await _context.Resgistrations
                .Include(r => r.Students)
                .Include(r => r.Events)
                .Include(r => r.Clubs)
                .FirstOrDefaultAsync(r => 
                    r.StudentId == studentId && 
                    r.Type == type.ToUpper() &&
                    (type.ToUpper() == "EVENT" ? r.EventId == activityId : r.ClubId == activityId));

            if (registration == null)
                return ResgistrationResult<ResgistrationDto>.Failure("Không tìm thấy đăng ký");

            return ResgistrationResult<ResgistrationDto>.Success(registration.ToDto());
        }
    }
}
