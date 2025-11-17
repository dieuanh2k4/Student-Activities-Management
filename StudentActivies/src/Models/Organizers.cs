using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Organizers 
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly Birth { get; set; }
        public string? Email { get; set; }
        // public int ClubId { get; set; }
        // public int EventId { get; set; }
        public int UserId { get; set; }

        public Users? Users { get; set; }
        public List<Clubs>? Clubs { get; set; }
        public List<Events>? Events { get; set; }
        public List<Report>? Reports { get; set; }
    }
}