using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentActivities.src.Controllers;
using StudentActivities.src.Services.Interfaces;
using StudentActivities.src.Dtos.Events;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace StudentActivities.Tests.Controllers
{
    /// <summary>
    /// Unit tests cho EventsController - Test CRUD operations for events
    /// </summary>
    public class EventsControllerTests
    {
        private readonly Mock<IEventService> _mockEventService;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly Mock<ILogger<EventsController>> _mockLogger;
        private readonly EventsController _controller;

        public EventsControllerTests()
        {
            _mockEventService = new Mock<IEventService>();
            _mockNotificationService = new Mock<INotificationService>();
            _mockLogger = new Mock<ILogger<EventsController>>();

            _controller = new EventsController(
                _mockEventService.Object,
                _mockNotificationService.Object,
                _mockLogger.Object
            );
        }        [Fact]
        public async Task GetAll_ReturnsOkWithEventsList()
        {
            // Arrange
            var expectedEvents = new List<EventDto>
            {
                new EventDto { Id = 1, Name = "Event 1", Description = "Description 1" },
                new EventDto { Id = 2, Name = "Event 2", Description = "Description 2" }
            };

            _mockEventService
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(expectedEvents);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var returnedEvents = okResult!.Value as List<EventDto>;

            returnedEvents.Should().HaveCount(2);
            returnedEvents.Should().BeEquivalentTo(expectedEvents);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsOkWithEvent()
        {
            // Arrange
            var eventId = 1;
            var expectedEvent = new EventDto
            {
                Id = eventId,
                Name = "Test Event",
                Description = "Test Description"
            };

            _mockEventService
                .Setup(s => s.GetByIdAsync(eventId))
                .ReturnsAsync(expectedEvent);

            // Act
            var result = await _controller.GetById(eventId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedEvent);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = 999;
            _mockEventService
                .Setup(s => s.GetByIdAsync(invalidId))
                .ReturnsAsync((EventDto?)null);

            // Act
            var result = await _controller.GetById(invalidId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Update_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var eventId = 1;
            var updateDto = new UpdateEventDto
            {
                Name = "Updated Event",
                Description = "Updated Description"
            };

            var existingEvent = new EventDto
            {
                Id = eventId,
                Name = "Original Event"
            };

            _mockEventService
                .Setup(s => s.GetByIdAsync(eventId))
                .ReturnsAsync(existingEvent);

            _mockEventService
                .Setup(s => s.UpdateAsync(eventId, updateDto))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(eventId, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockEventService.Verify(s => s.UpdateAsync(eventId, updateDto), Times.Once);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var eventId = 1;
            var existingEvent = new EventDto
            {
                Id = eventId,
                Name = "Event to Delete"
            };

            _mockEventService
                .Setup(s => s.GetByIdAsync(eventId))
                .ReturnsAsync(existingEvent);

            _mockEventService
                .Setup(s => s.DeleteAsync(eventId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(eventId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockEventService.Verify(s => s.DeleteAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = 999;
            _mockEventService
                .Setup(s => s.GetByIdAsync(invalidId))
                .ReturnsAsync((EventDto?)null);

            // Act
            var result = await _controller.Delete(invalidId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
