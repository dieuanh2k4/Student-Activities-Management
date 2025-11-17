using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Controllers;
using StudentActivities.src.Services.Interfaces;
using StudentActivities.src.Dtos.Auth;
using System.Threading.Tasks;

namespace StudentActivities.Tests.Controllers
{
    /// <summary>
    /// Unit tests cho AuthController - Test authentication logic
    /// </summary>
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                UserName = "testuser",
                Password = "Password123!"
            };

            var expectedResponse = new LoginResponseDto
            {
                Token = "fake-jwt-token",
                UserName = "testuser",
                Role = "Student"
            };

            _mockAuthService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(expectedResponse);
            
            _mockAuthService.Verify(
                s => s.LoginAsync(It.Is<LoginRequestDto>(r => 
                    r.UserName == loginRequest.UserName && 
                    r.Password == loginRequest.Password
                )), 
                Times.Once
            );
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                UserName = "wronguser",
                Password = "wrongpassword"
            };

            _mockAuthService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync((LoginResponseDto)null);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Login_WithNullUserName_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                UserName = null,
                Password = "somepassword"
            };

            _mockAuthService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync((LoginResponseDto)null);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task Login_WithEmptyPassword_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                UserName = "testuser",
                Password = ""
            };

            _mockAuthService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync((LoginResponseDto)null);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }
    }
}
