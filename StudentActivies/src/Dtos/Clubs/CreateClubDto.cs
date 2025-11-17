using System;

namespace StudentActivities.src.Dtos.Clubs
{
    public class CreateClubDto
    {
        public string Name { get; set; } = null!;
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public int OrganizerId { get; set; }
    }
}