using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Dtos.Statistics;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public StatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. THỐNG KÊ SINH VIÊN THEO LỚP, KHOA
        public async Task<StudentStatisticsDto> GetStudentStatisticsAsync(StudentStatisticsFilterDto? filter = null)
        {
            var query = _context.Students
                .Include(s => s.AcademicClasses)
                    .ThenInclude(ac => ac!.Faculties)
                .AsQueryable();

            if (filter?.ClassId.HasValue == true)
                query = query.Where(s => s.AcademicClassId == filter.ClassId.Value);

            if (filter?.FacultyId.HasValue == true)
                query = query.Where(s => s.FacultyId == filter.FacultyId.Value);

            var students = await query.ToListAsync();
            var totalStudents = students.Count;

            // Thống kê theo lớp
            var byClass = students
                .Where(s => s.AcademicClasses != null)
                .GroupBy(s => new
                {
                    ClassId = s.AcademicClassId,
                    ClassName = s.AcademicClasses!.Name ?? "Unknown"
                })
                .Select(g => new StudentByClassDto
                {
                    ClassId = g.Key.ClassId,
                    ClassName = g.Key.ClassName,
                    StudentCount = g.Count(),
                    Percentage = totalStudents > 0 ? Math.Round((double)g.Count() / totalStudents * 100, 2) : 0
                })
                .OrderByDescending(x => x.StudentCount)
                .ToList();

            // Thống kê theo khoa
            var byFaculty = students
                .Where(s => s.AcademicClasses?.Faculties != null)
                .GroupBy(s => new
                {
                    FacultyId = s.FacultyId,
                    FacultyName = s.AcademicClasses!.Faculties!.Name ?? "Unknown"
                })
                .Select(g => new StudentByFacultyDto
                {
                    FacultyId = g.Key.FacultyId,
                    FacultyName = g.Key.FacultyName,
                    StudentCount = g.Count(),
                    Percentage = totalStudents > 0 ? Math.Round((double)g.Count() / totalStudents * 100, 2) : 0
                })
                .OrderByDescending(x => x.StudentCount)
                .ToList();

            return new StudentStatisticsDto
            {
                TotalStudents = totalStudents,
                ByClass = byClass,
                ByFaculty = byFaculty,
                BySemester = new List<StudentBySemesterDto>()
            };
        }

        // 2. THỐNG KÊ SỰ KIỆN THEO NGÀY/THÁNG/NĂM
        public async Task<EventStatisticsDto> GetEventStatisticsByDateAsync(EventStatisticsFilterDto filter)
        {
            var query = _context.Events.AsQueryable();

            if (filter.StartDate.HasValue)
                query = query.Where(e => e.StartDate >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(e => e.StartDate <= filter.EndDate.Value);

            var events = await query.ToListAsync();
            var totalEvents = events.Count;

            List<EventByDateDto> byDate = new();

            switch (filter.Period.ToLower())
            {
                case "daily":
                    byDate = events
                        .GroupBy(e => e.StartDate.Date)
                        .Select(g => new EventByDateDto
                        {
                            Date = g.Key,
                            DateLabel = g.Key.ToString("yyyy-MM-dd"),
                            EventCount = g.Count(),
                            TotalParticipants = g.Sum(e => e.CurrentRegistrations),
                            Events = g.Select(e => new EventSummaryDto
                            {
                                EventId = e.Id,
                                EventName = e.Name ?? "Unknown",
                                StartDate = e.StartDate,
                                EndDate = e.EndDate,
                                ParticipantCount = e.CurrentRegistrations,
                                Status = GetEventStatus(e.StartDate, e.EndDate)
                            }).ToList()
                        })
                        .OrderBy(x => x.Date)
                        .ToList();
                    break;

                case "monthly":
                    byDate = events
                        .GroupBy(e => new DateTime(e.StartDate.Year, e.StartDate.Month, 1))
                        .Select(g => new EventByDateDto
                        {
                            Date = g.Key,
                            DateLabel = g.Key.ToString("yyyy-MM"),
                            EventCount = g.Count(),
                            TotalParticipants = g.Sum(e => e.CurrentRegistrations),
                            Events = g.Select(e => new EventSummaryDto
                            {
                                EventId = e.Id,
                                EventName = e.Name ?? "Unknown",
                                StartDate = e.StartDate,
                                EndDate = e.EndDate,
                                ParticipantCount = e.CurrentRegistrations,
                                Status = GetEventStatus(e.StartDate, e.EndDate)
                            }).ToList()
                        })
                        .OrderBy(x => x.Date)
                        .ToList();
                    break;

                case "yearly":
                    byDate = events
                        .GroupBy(e => new DateTime(e.StartDate.Year, 1, 1))
                        .Select(g => new EventByDateDto
                        {
                            Date = g.Key,
                            DateLabel = g.Key.ToString("yyyy"),
                            EventCount = g.Count(),
                            TotalParticipants = g.Sum(e => e.CurrentRegistrations),
                            Events = g.Select(e => new EventSummaryDto
                            {
                                EventId = e.Id,
                                EventName = e.Name ?? "Unknown",
                                StartDate = e.StartDate,
                                EndDate = e.EndDate,
                                ParticipantCount = e.CurrentRegistrations,
                                Status = GetEventStatus(e.StartDate, e.EndDate)
                            }).ToList()
                        })
                        .OrderBy(x => x.Date)
                        .ToList();
                    break;
            }

            return new EventStatisticsDto
            {
                TotalEvents = totalEvents,
                ByDate = byDate,
                Period = filter.Period
            };
        }

        // 2b. THỐNG KÊ CÂU LẠC BỘ
        public async Task<ClubStatisticsDto> GetClubStatisticsByDateAsync(ClubStatisticsFilterDto filter)
        {
            var clubs = await _context.Clubs
                .Include(c => c.Resgistrations)
                .ToListAsync();

            var totalClubs = clubs.Count;

            List<ClubByDateDto> byDate = new()
            {
                new ClubByDateDto
                {
                    Date = DateTime.Now.Date,
                    DateLabel = "All Time",
                    ClubCount = totalClubs,
                    TotalMembers = clubs.Sum(c => c.CurrentRegistrations),
                    Clubs = clubs.Select(c => new ClubSummaryDto
                    {
                        ClubId = c.Id,
                        ClubName = c.Name ?? "Unknown",
                        CreatedDate = DateTime.Now,
                        MemberCount = c.CurrentRegistrations,
                        EventCount = 0 // Clubs không có navigation property Events trong DB
                    }).ToList()
                }
            };

            return new ClubStatisticsDto
            {
                TotalClubs = totalClubs,
                ByDate = byDate,
                Period = "all"
            };
        }

        // 3. THỐNG KÊ SỰ KIỆN THEO TRẠNG THÁI
        public async Task<EventStatusStatisticsDto> GetEventStatisticsByStatusAsync(EventStatusFilterDto? filter = null)
        {
            var query = _context.Events
                .Include(e => e.Organizers)
                .AsQueryable();

            if (filter?.FromDate.HasValue == true)
                query = query.Where(e => e.StartDate >= filter.FromDate.Value);

            if (filter?.ToDate.HasValue == true)
                query = query.Where(e => e.EndDate <= filter.ToDate.Value);

            if (filter?.OrganizerId.HasValue == true)
                query = query.Where(e => e.OrganizerId == filter.OrganizerId.Value);

            var events = await query.ToListAsync();
            var totalEvents = events.Count;

            var eventsByStatus = events
                .Select(e => new
                {
                    Event = e,
                    Status = GetEventStatus(e.StartDate, e.EndDate)
                })
                .GroupBy(x => x.Status)
                .Select(g => new EventByStatusDto
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Percentage = totalEvents > 0 ? Math.Round((double)g.Count() / totalEvents * 100, 2) : 0,
                    Events = g.Select(x => new EventDetailDto
                    {
                        EventId = x.Event.Id,
                        EventName = x.Event.Name ?? "Unknown",
                        StartDate = x.Event.StartDate,
                        EndDate = x.Event.EndDate,
                        Location = x.Event.Location ?? "Unknown",
                        RegisteredCount = x.Event.CurrentRegistrations,
                        MaxParticipants = x.Event.MaxCapacity,
                        OrganizerName = x.Event.Organizers != null
                            ? $"{x.Event.Organizers.FirstName} {x.Event.Organizers.LastName}"
                            : "Unknown"
                    }).ToList()
                })
                .ToList();

            if (!string.IsNullOrEmpty(filter?.Status))
            {
                eventsByStatus = eventsByStatus
                    .Where(s => s.Status.Equals(filter.Status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var statusCounts = eventsByStatus.ToDictionary(x => x.Status, x => x.Count);

            return new EventStatusStatisticsDto
            {
                TotalEvents = totalEvents,
                ByStatus = eventsByStatus,
                StatusCounts = statusCounts
            };
        }

        private string GetEventStatus(DateTime startDate, DateTime endDate)
        {
            var now = DateTime.Now;

            if (now < startDate)
                return "Upcoming";
            else if (now >= startDate && now <= endDate)
                return "Ongoing";
            else
                return "Completed";
        }
    }
}