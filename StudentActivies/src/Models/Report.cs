using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public int AdminId { get; set; }
        public int OrganizerId { get; set; }

        public Admins? Admins { get; set; }
        public Organizers? Organizers { get; set; }
    }
}