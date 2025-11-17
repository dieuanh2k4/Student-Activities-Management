using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Services.Interfaces;
using StudentActivities.src.Services.Extensions;

namespace StudentActivities.src.BackgroundServices
{
    public class EventReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventReminderService> _logger;

        public EventReminderService(IServiceProvider serviceProvider, ILogger<EventReminderService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Đợi 10 giây để đảm bảo migrations đã hoàn tất
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SendEventReminders();

                    // Chạy mỗi 12 tiếng (2 lần mỗi ngày)
                    await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi trong quá trình gửi thông báo nhắc nhở sự kiện");

                    // Chờ 1 tiếng trước khi thử lại nếu có lỗi
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
            }
        }

        private async Task SendEventReminders()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var utcNow = DateTime.UtcNow;

            // Lấy tất cả sự kiện trong 3 ngày tới và filter trong memory
            var startDate = utcNow.Date;
            var endDate = utcNow.Date.AddDays(3);

            var allUpcomingEvents = await context.Events
                .Where(e => e.StartDate >= startDate && e.StartDate <= endDate)
                .ToListAsync();

            // Filter trong memory để tránh vấn đề DateTime Kind
            var upcomingEvents = allUpcomingEvents
                .Where(e =>
                {
                    var eventDate = e.StartDate.Date;
                    var tomorrowDate = utcNow.Date.AddDays(1);
                    var dayAfterTomorrowDate = utcNow.Date.AddDays(2);
                    return eventDate >= tomorrowDate && eventDate <= dayAfterTomorrowDate;
                })
                .ToList(); foreach (var eventItem in upcomingEvents)
            {
                try
                {
                    // Kiểm tra xem đã gửi thông báo nhắc nhở trong 24h qua chưa
                    var yesterday = DateOnly.FromDateTime(utcNow.AddDays(-1));
                    var today = DateOnly.FromDateTime(utcNow);

                    var recentReminder = await context.Notifications
                        .Where(n => n.EventId == eventItem.Id &&
                                   n.Context != null && n.Context.Contains("⏰ Nhắc nhở") &&
                                   n.SendDate >= yesterday)
                        .AnyAsync();

                    if (!recentReminder)
                    {
                        await notificationService.SendEventNotificationAsync(
                            eventItem.Id,
                            eventItem.Name ?? "Sự kiện",
                            NotificationType.EventReminder);

                        _logger.LogInformation($"Đã gửi thông báo nhắc nhở cho sự kiện: {eventItem.Name}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Lỗi khi gửi thông báo nhắc nhở cho sự kiện {eventItem.Id}");
                }
            }

            if (upcomingEvents.Any())
            {
                _logger.LogInformation($"Đã xử lý {upcomingEvents.Count} sự kiện sắp diễn ra");
            }
        }
    }
}