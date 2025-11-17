using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Resgistrations
    {
        public int Id { get; set; }
        public string Status { get; set; } = "REGISTERED"; // REGISTERED, CHECKED_IN, ATTENDED, COMPLETED, CANCELLED
        public DateTime RegistrationDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CancellationTime { get; set; }
        public decimal? PointsEarned { get; set; }
        public string? Notes { get; set; }
        
        public int StudentId { get; set; }
        public int? ClubId { get; set; }
        public int? EventId { get; set; }
        public string Type { get; set; } = "EVENT"; // EVENT hoặc CLUB

        public Students? Students { get; set; }
        public Clubs? Clubs { get; set; }
        public Events? Events { get; set; }
    }
}