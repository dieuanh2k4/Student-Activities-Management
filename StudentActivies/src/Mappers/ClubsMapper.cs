using StudentActivities.src.Dtos.Clubs;
using StudentActivities.src.Dtos.Organizers;  // <-- THÊM DÒNG NÀY
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class ClubsMapper
    {
        public static ClubDto ToDto(Clubs c)
        {
            if (c == null) return null!;

            string? organizerName = null;
            if (c.Organizers != null)
            {
                organizerName = $"{c.Organizers.FirstName} {c.Organizers.LastName}".Trim();
            }

            return new ClubDto
            {
                Id = c.Id,
                Name = c.Name,
                Thumbnail = c.Thumbnail,
                Description = c.Description,
                MaxCapacity = c.MaxCapacity,
                CurrentMembers = c.Resgistrations?.Count ?? 0,
                OrganizerId = c.OrganizerId,
                OrganizerName = organizerName ?? "Không xác định",
            };
        }

        public static ClubSummaryDto ToSummaryDto(Clubs c)
        {
            if (c == null) return null!;
            return new ClubSummaryDto
            {
                Id = c.Id,
                Name = c.Name,
                Thumbnail = c.Thumbnail,
                MemberCount = c.Resgistrations?.Count ?? 0
            };
        }
    }
}
