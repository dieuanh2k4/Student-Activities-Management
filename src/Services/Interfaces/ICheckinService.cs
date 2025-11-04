using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentActivities.src.Dtos.Checkins;

namespace StudentActivities.src.Services.Interfaces
{
    public interface ICheckinService
    {
        /// <summary>
        /// Lấy danh sách sinh viên đã đăng ký sự kiện (Chức năng 1)
        /// </summary>
        Task<List<CheckinDto>> GetEventRegistrationsAsync(int eventId);

        /// <summary>
        /// Xem trạng thái check-in của sinh viên trong sự kiện (Chức năng 2)
        /// </summary>
        Task<List<CheckinDto>> GetEventCheckinsAsync(int eventId);

        /// <summary>
        /// Cập nhật trạng thái check-in cho sinh viên (Chức năng 3 - Manual)
        /// </summary>
        Task<CheckinDto> UpdateCheckinStatusAsync(int eventId, int studentId, UpdateCheckinStatusDto dto, int userId);

        /// <summary>
        /// Tìm kiếm sinh viên trong sự kiện theo mã SV hoặc tên
        /// </summary>
        Task<List<CheckinDto>> SearchStudentsInEventAsync(int eventId, string query);

        /// <summary>
        /// Lấy thống kê check-in của sự kiện
        /// </summary>
        Task<CheckinStatisticsDto> GetCheckinStatisticsAsync(int eventId);

        /// <summary>
        /// Check-in hàng loạt (Admin only)
        /// </summary>
        Task<List<CheckinDto>> BulkCheckinAsync(int eventId, List<int> studentIds, int userId);
    }
}
