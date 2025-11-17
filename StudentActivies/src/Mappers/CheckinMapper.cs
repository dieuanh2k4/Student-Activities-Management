using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentActivities.src.Dtos.Checkins;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class CheckinMapper
    {
        public static CheckinDto ToCheckinDto(this Checkin checkin)
        {
            var studentFullName = $"{checkin.Students?.FirstName} {checkin.Students?.LastName}".Trim();
            var checkedInByName = checkin.CheckedInByUser != null
                ? (checkin.CheckedInByUser.Admins != null
                    ? $"{checkin.CheckedInByUser.Admins.FirstName} {checkin.CheckedInByUser.Admins.LastName}".Trim()
                    : checkin.CheckedInByUser.Organizers != null
                        ? $"{checkin.CheckedInByUser.Organizers.FirstName} {checkin.CheckedInByUser.Organizers.LastName}".Trim()
                        : null)
                : null;

            return new CheckinDto
            {
                Id = checkin.Id,
                StudentId = checkin.StudentId,
                StudentCode = checkin.Students?.Users?.UserName,
                StudentName = studentFullName,
                StudentEmail = checkin.Students?.Email,
                StudentPhone = checkin.Students?.PhoneNumber,
                ClassName = checkin.Students?.AcademicClasses?.Name,
                FacultyName = null, // Will be populated if needed
                EventId = checkin.EventId,
                EventName = checkin.Events?.Name,
                Status = checkin.Status ?? "Vắng mặt",
                CheckInTime = checkin.CheckInTime,
                CheckedInByName = checkedInByName
            };
        }

        public static CheckinDto ToCheckinDtoFromRegistration(this Resgistrations registration, int eventId)
        {
            var studentFullName = $"{registration.Students?.FirstName} {registration.Students?.LastName}".Trim();

            return new CheckinDto
            {
                Id = 0, // Will be set when checkin record is created
                StudentId = registration.StudentId,
                StudentCode = registration.Students?.Users?.UserName,
                StudentName = studentFullName,
                StudentEmail = registration.Students?.Email,
                StudentPhone = registration.Students?.PhoneNumber,
                ClassName = registration.Students?.AcademicClasses?.Name,
                FacultyName = null,
                EventId = eventId,
                EventName = registration.Events?.Name,
                Status = "Vắng mặt",
                CheckInTime = null,
                CheckedInByName = null
            };
        }
    }
}
