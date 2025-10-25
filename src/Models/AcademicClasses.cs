using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Models
{
    public class AcademicClasses
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        // public int StudentId { get; set; }
        public int FacultyId { get; set; }

        public List<Students>? Students { get; set; }
        public Faculties? Faculties { get; set; }
    }
}