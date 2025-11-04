using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Dtos.Checkins
{
    public class CheckinStatisticsDto
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public int TotalRegistrations { get; set; }
        public int TotalAttended { get; set; }
        public int TotalAbsent { get; set; }
        public double AttendanceRate { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
