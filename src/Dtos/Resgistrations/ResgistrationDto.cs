using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Dtos.Resgistrations
{
    public class ResgistrationDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int? EventId { get; set; }
        public string? EventName { get; set; }
        public int? ClubId { get; set; }
        public string? ClubName { get; set; }
        public string Type { get; set; } = "EVENT";
        public string Status { get; set; } = "REGISTERED";
        public DateTime RegistrationDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CancellationTime { get; set; }
        public decimal? PointsEarned { get; set; }
        public string? Notes { get; set; }
    }
}
