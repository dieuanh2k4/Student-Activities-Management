using StudentActivities.src.Dtos.TrainingScores;
using StudentActivities.src.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StudentActivities.src.Mappers
{
    public static class TrainingScoreMapper
    {
        public static TrainingScoreDto ToDto(this TrainingScores trainingScore)
        {
            var student = trainingScore.Students;
            var studentName = student != null
                ? $"{student.FirstName} {student.LastName}".Trim()
                : null;

            return new TrainingScoreDto
            {
                Id = trainingScore.Id,
                Score = trainingScore.Score,
                DateAssigned = trainingScore.DateAssigned,
                EventId = trainingScore.EventId,
                StudentId = trainingScore.StudentId,
                SemesterId = trainingScore.SemesterId,
                EventName = trainingScore.Events?.Name,
                StudentName = studentName,
                StudentCode = student?.Users?.UserName,
                SemesterName = trainingScore.Semester?.Name,
                FacultyName = student?.AcademicClasses?.Faculties?.Name,
                ClassName = trainingScore.Students?.AcademicClasses?.Name
            };
        }
    }
}
