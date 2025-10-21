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
        public int UserId { get; set; }

        public Users? Users { get; set; }
    }
}