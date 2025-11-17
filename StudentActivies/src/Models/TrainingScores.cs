using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class TrainingScores
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateOnly DateAssigned { get; set; }
        public int EventId { get; set; }
        public int StudentId { get; set; }
        public int SemesterId { get; set; }

        public Events? Events { get; set; }
        public Students? Students { get; set; }
        public Semester? Semester { get; set; }
    }
}