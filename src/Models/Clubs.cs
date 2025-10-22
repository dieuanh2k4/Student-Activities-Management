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
        public string? Description { get; set; }
        public int UserId { get; set; }
        
        public Users? Users { get; set; }
        public List<Notifications>? Notifications { get; set; }

    }
}