using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Admins
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly Birth { get; set; }
        public string? Email { get; set; }
        // public string Role { get; set; } = "Admin";
        public int UserId { get; set; }

        public Users? Users { get; set; }
        public List<Report>? Reports { get; set; }
    }
}