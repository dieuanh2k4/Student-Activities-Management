using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentActivities.src.Models;

namespace StudentActivities.src.Models
{
    public class Clubs
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int OrganizerId { get; set; } // Thêm FK
        public Organizers? Organizers { get; set; }

        public List<Notifications>? Notifications { get; set; }
        public List<Registrations>? Registrations { get; set; }
    }
}