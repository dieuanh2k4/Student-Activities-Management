using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Dtos.Resgistrations
{
    public class CreateResgistrationDto
    {
        [Required(ErrorMessage = "EventId hoặc ClubId là bắt buộc")]
        public int? EventId { get; set; }
        
        public int? ClubId { get; set; }
        
        [Required]
        public string Type { get; set; } = "EVENT"; // EVENT hoặc CLUB
    }
}
