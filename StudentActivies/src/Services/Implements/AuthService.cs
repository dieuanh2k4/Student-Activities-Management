using StudentActivities.src.Dtos.Auth;
using StudentActivities.src.Repositories.Interfaces;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtTokenService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepo, 
            IJwtTokenService jwtService,
            ILogger<AuthService> logger)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepo.GetByUserNameAsync(request.UserName);
            
            // Verify password với BCrypt
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                _logger.LogWarning("Login failed for user {UserName}", request.UserName);
                return null;
            }

            // Tạo JWT token thông qua JwtTokenService
            var token = _jwtService.GenerateAccessToken(user);

            _logger.LogInformation("User {UserName} logged in successfully", user.UserName);

            return new LoginResponseDto
            {
                UserName = user.UserName!,
                Role = user.Role!,
                Token = token
            };
        }
    }
}
