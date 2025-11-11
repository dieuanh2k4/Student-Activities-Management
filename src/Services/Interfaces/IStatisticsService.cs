using StudentActivities.src.Dtos.Statistics;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IStatisticsService
    {
        // Thống kê sinh viên
        Task<StudentStatisticsDto> GetStudentStatisticsAsync(StudentStatisticsFilterDto? filter = null);

        // Thống kê sự kiện theo ngày/tháng/năm
        Task<EventStatisticsDto> GetEventStatisticsByDateAsync(EventStatisticsFilterDto filter);

        // Thống kê câu lạc bộ theo ngày/tháng/năm
        Task<ClubStatisticsDto> GetClubStatisticsByDateAsync(ClubStatisticsFilterDto filter);

        // Thống kê sự kiện theo trạng thái
        Task<EventStatusStatisticsDto> GetEventStatisticsByStatusAsync(EventStatusFilterDto? filter = null);
    }
}