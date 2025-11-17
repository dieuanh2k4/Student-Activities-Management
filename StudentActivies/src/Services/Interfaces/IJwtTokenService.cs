using System.Security.Claims;
using StudentActivities.src.Models;

namespace StudentActivities.src.Services.Interfaces
{
    /// <summary>
    /// Service xử lý tạo và validate JWT tokens
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Tạo Access Token (JWT) cho user
        /// </summary>
        /// <param name="user">Thông tin user cần tạo token</param>
        /// <returns>JWT token string</returns>
        string GenerateAccessToken(Users user);

        /// <summary>
        /// Lấy thời gian expire của token (phút)
        /// </summary>
        int GetTokenExpiryMinutes();

        /// <summary>
        /// Validate token và trả về ClaimsPrincipal (nếu cần custom validation)
        /// </summary>
        ClaimsPrincipal? ValidateToken(string token);
    }
}
