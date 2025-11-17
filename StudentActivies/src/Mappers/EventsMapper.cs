using StudentActivities.src.Dtos.Events;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class EventsMapper
    {
        public static EventDto ToDto(Events e)
        {
            if (e == null) return null!;
            return new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                Thumbnail = e.Thumbnail,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Location = e.Location,
                MaxCapacity = e.MaxCapacity,
                CurrentRegistrations = e.CurrentRegistrations,
                Status = e.Status,
                OrganizerId = e.OrganizerId
            };
        }
    }
}