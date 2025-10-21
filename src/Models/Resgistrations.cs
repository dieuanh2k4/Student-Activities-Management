using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class Resgistrations
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public DateOnly DateResgistered { get; set; }

        
    }
}