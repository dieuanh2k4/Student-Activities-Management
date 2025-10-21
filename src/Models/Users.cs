using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly Birth { get; set; }
        public string? Email { get; set; }
        public int Role { get; set; }
        public string? Faculty { get; set; }
        public string? Class { get; set; }
        public string? Semester { get; set; }

        public List<Clubs>? Clubs { get; set; }
        public List<Events>? Events { get; set; }
        public TrainingScores? TrainingScores { get; set; }
    }
}