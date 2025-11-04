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
        public string? Thumbnail { get; set; } // ảnh sự kiện
        public string? Paticipants { get; set; } // người tham gia
        public string? Description { get; set; } // mô tả sự kiện
        public string? DetailDescription { get; set; } // mô tả chi tiết sự kiện
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; } // địa điểm
        public int MaxCapacity { get; set; } // số người tham gia
        public int CurrentRegistrations { get; set; } // số người đăng kí hiện tại
        public DateTime RegistrationEndDate { get; set; } // Hạn chót đăng ký
        public decimal TrainingPoints { get; set; } // Điểm rèn luyện
        public string? Status
        {
            get
            {
                var today = DateTime.Today;
                if (today < StartDate) return "Sắp diễn ra";
                if (today > EndDate) return "Đã kết thúc";
                return "Đang diễn ra";
            }
        }
        public int OrganizerId { get; set; }

        public Organizers? Organizers { get; set; }
        public List<Resgistrations>? Resgistrations { get; set; }
        public List<Notifications>? Notifications { get; set; }
        public TrainingScores? TrainingScores { get; set; }
        public Checkin? Checkin { get; set; }
    }
}