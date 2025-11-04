using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentActivities.src.Dtos.Resgistrations;
using StudentActivities.src.Exceptions;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IResgistrationService
    {
        // Danh sách sự kiện/CLB có thể đăng ký
        Task<ResgistrationResult<List<AvailableActivityDto>>> GetAvailableActivitiesAsync(int studentId, string? type = null);
        
        // Đăng ký sự kiện/CLB
        Task<ResgistrationResult<ResgistrationDto>> RegisterActivityAsync(int studentId, CreateResgistrationDto dto);
        
        // Hủy đăng ký
        Task<ResgistrationResult<bool>> CancelRegistrationAsync(int studentId, int activityId, string type);
        
        // Xem danh sách đã đăng ký của sinh viên
        Task<ResgistrationResult<List<ResgistrationDto>>> GetMyRegistrationsAsync(int studentId, string? type = null, string? status = null);
        
        // Xem chi tiết 1 đăng ký
        Task<ResgistrationResult<ResgistrationDto>> GetRegistrationDetailAsync(int studentId, int activityId, string type);
    }
}
