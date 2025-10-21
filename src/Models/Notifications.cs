using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Notifications
    {
        public int Id { get; set; }
        public string? DbContextOptions { get; set; }
        public DateOnly SendDate { get; set; }

        
    }
}