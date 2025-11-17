using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentActivities.src.Dtos.Organizers;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class OrganizerMapper
    {
        public static async Task<Organizers> ToOrganizerFromCreateDto(this CreateOrganizerDto createOrganizerDto, int userid)
        {
            return new Organizers
            {
                FirstName = createOrganizerDto.FirstName,
                LastName = createOrganizerDto.LastName,
                PhoneNumber = createOrganizerDto.PhoneNumber,
                Birth = createOrganizerDto.Birth,
                Email = createOrganizerDto.Email,
                UserId = userid
            };
        }
    }
}