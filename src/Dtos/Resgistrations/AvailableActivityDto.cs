using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Dtos.Resgistrations
{
    public class AvailableActivityDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Type { get; set; } = "EVENT"; // EVENT hoặc CLUB
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentRegistrations { get; set; }
        public int AvailableSlots => MaxCapacity - CurrentRegistrations;
        public decimal TrainingPoints { get; set; }
        public string? Status { get; set; }
        public bool CanRegister { get; set; }
        public bool IsRegistered { get; set; }
        public string? RegistrationMessage { get; set; }
    }
}
