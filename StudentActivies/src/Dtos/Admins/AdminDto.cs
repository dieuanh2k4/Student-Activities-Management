using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Dtos.Admins
{
    public class AdminDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly Birth { get; set; }
        public string? Email { get; set; }
        public int UserId { get; set; }
    }
}