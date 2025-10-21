using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Events
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Location { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentRegistrations { get; set; }
        public string? Status { get; set; }
        public int UserId { get; set; }

        public Users? Users { get; set; }
        public List<Notifications>? Notifications { get; set; }
    }
}