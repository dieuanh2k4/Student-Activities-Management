using System;

namespace StudentActivities.src.Dtos.Events
{
    public class EventDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentRegistrations { get; set; }
        public string? Status { get; set; }
        public int OrganizerId { get; set; }
    }
}