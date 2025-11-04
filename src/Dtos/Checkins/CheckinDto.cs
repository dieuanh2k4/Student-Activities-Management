using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Dtos.Checkins
{
    public class CheckinDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string? StudentCode { get; set; }
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }
        public string? StudentPhone { get; set; }
        public string? ClassName { get; set; }
        public string? FacultyName { get; set; }
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string Status { get; set; } = "Vắng mặt";
        public DateTime? CheckInTime { get; set; }
        public string? CheckedInByName { get; set; }
    }
}
