using System;

namespace StudentActivities.src.Dtos.Clubs
{
    public class ClubDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public int OrganizerId { get; set; }
    }
}