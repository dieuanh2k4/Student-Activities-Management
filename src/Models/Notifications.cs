using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Notifications
    {
        public int Id { get; set; }
        public string? Context { get; set; }
        public DateOnly SendDate { get; set; }
        public int EventId { get; set; }
        public int ClubId { get; set; }

        public Events? Events { get; set; }
        public Clubs? Clubs { get; set; }
    }
}