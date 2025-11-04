using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Students 
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly Birth { get; set; }
        public string? Email { get; set; }
        public int UserId { get; set; }
        public int AcademicClassId { get; set; }
        public int FacultyId { get; set; }
        public int TrainingScore { get; set; }

        public Users? Users { get; set; }
        public List<Resgistrations>? Resgistrations { get; set; }
        public AcademicClasses? AcademicClasses { get; set; }
        public List<TrainingScores>? TrainingScores { get; set; }
        public List<Notifications>? Notifications { get; set; }
        public Checkin? Checkin { get; set; }
    }
}