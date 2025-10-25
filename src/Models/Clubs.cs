using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Clubs
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public int MaxCapacity { get; set; } // số lượng người tham gia tối đa
        public int OrganizerId { get; set; }
        
        public Organizers? Organizers { get; set; }
        public List<Notifications>? Notifications { get; set; }
        public Resgistrations? Resgistrations { get; set; }
    }
}