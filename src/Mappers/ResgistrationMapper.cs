using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentActivities.src.Dtos.Resgistrations;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class ResgistrationMapper
    {
        public static ResgistrationDto ToDto(this Resgistrations resgistration)
        {
            return new ResgistrationDto
            {
                Id = resgistration.Id,
                StudentId = resgistration.StudentId,
                StudentName = resgistration.Students != null 
                    ? $"{resgistration.Students.FirstName} {resgistration.Students.LastName}" 
                    : null,
                EventId = resgistration.EventId,
                EventName = resgistration.Events?.Name,
                ClubId = resgistration.ClubId,
                ClubName = resgistration.Clubs?.Name,
                Type = resgistration.Type,
                Status = resgistration.Status,
                RegistrationDate = resgistration.RegistrationDate,
                CheckInTime = resgistration.CheckInTime,
                CancellationTime = resgistration.CancellationTime,
                PointsEarned = resgistration.PointsEarned,
                Notes = resgistration.Notes
            };
        }

        public static AvailableActivityDto ToAvailableActivityDto(this Events eventItem, bool isRegistered = false)
        {
            var now = DateTime.Now;
            var canRegister = now <= eventItem.RegistrationEndDate 
                && eventItem.CurrentRegistrations < eventItem.MaxCapacity 
                && !isRegistered;

            string message = "";
            if (isRegistered)
                message = "Bạn đã đăng ký";
            else if (now > eventItem.RegistrationEndDate)
                message = "Đã hết hạn đăng ký";
            else if (eventItem.CurrentRegistrations >= eventItem.MaxCapacity)
                message = "Đã đầy";
            else
                message = "Có thể đăng ký";

            return new AvailableActivityDto
            {
                Id = eventItem.Id,
                Name = eventItem.Name,
                Type = "EVENT",
                Description = eventItem.Description,
                Thumbnail = eventItem.Thumbnail,
                Location = eventItem.Location,
                StartDate = eventItem.StartDate,
                EndDate = eventItem.EndDate,
                RegistrationEndDate = eventItem.RegistrationEndDate,
                MaxCapacity = eventItem.MaxCapacity,
                CurrentRegistrations = eventItem.CurrentRegistrations,
                TrainingPoints = eventItem.TrainingPoints,
                Status = eventItem.Status,
                CanRegister = canRegister,
                IsRegistered = isRegistered,
                RegistrationMessage = message
            };
        }

        public static AvailableActivityDto ToAvailableActivityDto(this Clubs club, bool isRegistered = false)
        {
            var now = DateTime.Now;
            var canRegister = now <= club.RegistrationEndDate 
                && club.CurrentRegistrations < club.MaxCapacity 
                && !isRegistered;

            string message = "";
            if (isRegistered)
                message = "Bạn đã đăng ký";
            else if (now > club.RegistrationEndDate)
                message = "Đã hết hạn đăng ký";
            else if (club.CurrentRegistrations >= club.MaxCapacity)
                message = "Đã đầy";
            else
                message = "Có thể đăng ký";

            return new AvailableActivityDto
            {
                Id = club.Id,
                Name = club.Name,
                Type = "CLUB",
                Description = club.Description,
                Thumbnail = club.Thumbnail,
                RegistrationEndDate = club.RegistrationEndDate,
                MaxCapacity = club.MaxCapacity,
                CurrentRegistrations = club.CurrentRegistrations,
                TrainingPoints = club.TrainingPoints,
                CanRegister = canRegister,
                IsRegistered = isRegistered,
                RegistrationMessage = message
            };
        }
    }
}
