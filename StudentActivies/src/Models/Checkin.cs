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
        public string? Status { get; set; } = "Vắng mặt"; // "Đã tham dự", "Vắng mặt"

        // Thời gian check-in thực tế
        public DateTime? CheckInTime { get; set; }

        // Người thực hiện check-in (Admin hoặc Organizer)
        public int? CheckedInBy { get; set; }

        public Students? Students { get; set; }
        public Events? Events { get; set; }
        public Users? CheckedInByUser { get; set; }
    }
}