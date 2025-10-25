using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Checkin
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int EventId { get; set; }
        public string? Status { get; set; } = "Vắng mặt";

        public Students? Students { get; set; }
        public Events? Events { get; set; }
    }
}