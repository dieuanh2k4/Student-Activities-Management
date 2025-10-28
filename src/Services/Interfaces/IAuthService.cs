using StudentActivities.src.Dtos.Auth;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}
