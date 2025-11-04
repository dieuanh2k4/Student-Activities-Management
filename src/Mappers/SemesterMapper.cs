using StudentActivities.src.Dtos.Semesters;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class SemesterMapper
    {
        public static SemesterDto ToDto(Semester semester)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            string status;

            if (today < semester.StartDate)
                status = "Chưa bắt đầu";
            else if (today > semester.EndDate)
                status = "Đã kết thúc";
            else
                status = "Đang diễn ra";

            return new SemesterDto
            {
                Id = semester.Id,
                Name = semester.Name ?? string.Empty,
                StartDate = semester.StartDate,
                EndDate = semester.EndDate,
                Status = status,
                DurationDays = semester.EndDate.DayNumber - semester.StartDate.DayNumber
            };
        }

        public static Semester ToEntity(CreateSemesterDto dto)
        {
            return new Semester
            {
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
        }

        public static void UpdateEntity(Semester semester, UpdateSemesterDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Name))
                semester.Name = dto.Name;

            if (dto.StartDate.HasValue)
                semester.StartDate = dto.StartDate.Value;

            if (dto.EndDate.HasValue)
                semester.EndDate = dto.EndDate.Value;
        }
    }
}