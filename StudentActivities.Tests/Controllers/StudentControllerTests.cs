using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Controllers;
using StudentActivities.src.Services.Interfaces;
using StudentActivities.src.Dtos.Students;
using StudentActivities.src.Models;
using StudentActivities.src.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace StudentActivities.Tests.Controllers
{
    /// <summary>
    /// Unit tests cho StudentController - Test student management
    /// </summary>
    public class StudentControllerTests
    {
        private readonly Mock<IStudentService> _mockStudentService;
        private readonly ApplicationDbContext _context;
        private readonly Mock<ILogger<StudentController>> _mockLogger;
        private readonly StudentController _controller;

        public StudentControllerTests()
        {
            // Create InMemory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new ApplicationDbContext(options);
            _mockStudentService = new Mock<IStudentService>();
            _mockLogger = new Mock<ILogger<StudentController>>();
            _controller = new StudentController(_context, _mockStudentService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllStudent_ReturnsOkWithStudentsList()
        {
            // Arrange
            var expectedStudents = new List<Students>
            {
                new Students { Id = 1, FirstName = "Nguyen", LastName = "Van A", Email = "nguyenvana@example.com" },
                new Students { Id = 2, FirstName = "Tran", LastName = "Thi B", Email = "tranthib@example.com" }
            };

            _mockStudentService
                .Setup(s => s.GetAllStudent())
                .ReturnsAsync(expectedStudents);

            // Act
            var result = await _controller.GetAllStudent();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var students = okResult!.Value as List<Students>;
            students.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreateStudent_WithValidData_ReturnsOkWithCreatedStudent()
        {
            // Arrange
            var createDto = new CreateStudentDto
            {
                FirstName = "Le",
                LastName = "Van C",
                Email = "levanc@example.com",
                PhoneNumber = "0123456789",
                Birth = new DateOnly(2000, 1, 1),
                UserId = 1,
                AcademicClassId = 1,
                FacultyId = 1
            };

            var createdStudent = new Students
            {
                Id = 1,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Email = createDto.Email,
                PhoneNumber = createDto.PhoneNumber,
                Birth = createDto.Birth,
                UserId = createDto.UserId,
                AcademicClassId = createDto.AcademicClassId,
                FacultyId = createDto.FacultyId
            };

            _mockStudentService
                .Setup(s => s.CreateStudent(It.IsAny<CreateStudentDto>(), It.IsAny<int>()))
                .ReturnsAsync(createdStudent);

            // Act
            var result = await _controller.CreateStudent(createDto, 1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            _mockStudentService.Verify(s => s.CreateStudent(It.IsAny<CreateStudentDto>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInforStudent_WithValidData_ReturnsOkWithUpdatedStudent()
        {
            // Arrange
            var studentId = 1;
            var updateDto = new UpdateStudentDto
            {
                FirstName = "Nguyen",
                LastName = "Van A Updated",
                Email = "updated@example.com",
                PhoneNumber = "0987654321",
                Birth = new DateOnly(2000, 1, 1),
                UserId = 1,
                AcademicClassId = 1,
                FacultyId = 1
            };

            var updatedStudent = new Students
            {
                Id = studentId,
                FirstName = updateDto.FirstName,
                LastName = updateDto.LastName,
                Email = updateDto.Email,
                PhoneNumber = updateDto.PhoneNumber,
                Birth = updateDto.Birth,
                UserId = updateDto.UserId,
                AcademicClassId = updateDto.AcademicClassId,
                FacultyId = updateDto.FacultyId
            };

            _mockStudentService
                .Setup(s => s.UpdateInforStudent(It.IsAny<UpdateStudentDto>(), studentId))
                .ReturnsAsync(updatedStudent);

            // Act
            var result = await _controller.UpdateInforStudent(updateDto, studentId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var student = okResult!.Value as Students;
            student!.LastName.Should().Be(updateDto.LastName);
        }

        [Fact]
        public async Task GetTrainingScore_WithValidId_ReturnsOkWithTrainingScores()
        {
            // Arrange
            var studentId = 1;
            var expectedScores = new List<TrainingScores>
            {
                new TrainingScores { Id = 1, StudentId = studentId, Score = 85 },
                new TrainingScores { Id = 2, StudentId = studentId, Score = 90 }
            };

            _mockStudentService
                .Setup(s => s.GetTrainingScore(studentId))
                .ReturnsAsync(expectedScores);

            // Act
            var result = await _controller.GetTrainingScore(studentId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var scores = okResult!.Value as List<TrainingScores>;
            scores.Should().HaveCount(2);
        }
    }
}
