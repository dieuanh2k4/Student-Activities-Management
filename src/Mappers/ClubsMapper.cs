using StudentActivities.src.Dtos.Clubs;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class ClubsMapper
    {
        public static ClubDto ToDto(Clubs c)
        {
            if (c == null) return null!;

            return new ClubDto
            {
                Id = c.Id,
                Name = c.Name,
                Thumbnail = c.Thumbnail,
                Description = c.Description,
                MaxCapacity = c.MaxCapacity,
                CurrentMembers = c.Registrations?.Count ?? 0,
                OrganizerId = c.OrganizerId,

                // ĐÃ SỬA: DÙNG FirstName + LastName
                OrganizerName = c.Organizers != null
                    ? $"{c.Organizers.FirstName} {c.Organizers.LastName}".Trim()
                    : null
            };
        }
    }
}