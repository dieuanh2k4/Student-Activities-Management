using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Dtos.Checkins
{
    public class UpdateCheckinStatusDto
    {
        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Đã tham dự|Vắng mặt)$", ErrorMessage = "Status must be 'Đã tham dự' or 'Vắng mặt'")]
        public string Status { get; set; } = string.Empty;
    }
}
