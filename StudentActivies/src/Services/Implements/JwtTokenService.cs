using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StudentActivities.src.Models;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    /// <summary>
    /// Service xử lý tạo và validate JWT tokens
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IConfiguration config, ILogger<JwtTokenService> logger)
        {
            _config = config;
            _logger = logger;
            ValidateConfiguration();
        }

        /// <summary>
        /// Tạo Access Token (JWT) cho user với các claims chuẩn
        /// </summary>
        public string GenerateAccessToken(Users user)
        {
            var claims = BuildClaims(user);
            var key = GetSigningKey();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(GetTokenExpiryMinutes()),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            
            _logger.LogInformation("Generated JWT token for user {UserName} (ID: {UserId})", 
                user.UserName, user.Id);

            return tokenString;
        }

        /// <summary>
        /// Lấy thời gian expire của token từ config
        /// </summary>
        public int GetTokenExpiryMinutes()
        {
            var expireMinutesStr = _config["Jwt:ExpireMinutes"] ?? "60";
            return int.TryParse(expireMinutesStr, out var minutes) ? minutes : 60;
        }

        /// <summary>
        /// Validate token và trả về ClaimsPrincipal
        /// </summary>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = GetSigningKey();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero // Không cho phép skew time
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return null;
            }
        }

        /// <summary>
        /// Tạo claims cho JWT token
        /// </summary>
        private IEnumerable<Claim> BuildClaims(Users user)
        {
            return new[]
            {
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Role, user.Role ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // User ID
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()) // Issued at
            };
        }

        /// <summary>
        /// Lấy signing key từ config
        /// </summary>
        private SymmetricSecurityKey GetSigningKey()
        {
            var key = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT Key is not configured in appsettings.json");
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// Validate JWT configuration khi khởi tạo service
        /// </summary>
        private void ValidateConfiguration()
        {
            var key = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("Jwt:Key is missing in configuration");
            }

            if (key.Length < 32)
            {
                _logger.LogWarning("JWT Key should be at least 32 characters for better security. Current length: {Length}", key.Length);
            }

            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            
            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("Jwt:Issuer and Jwt:Audience must be configured");
            }

            _logger.LogInformation("JWT configuration validated successfully");
        }
    }
}
