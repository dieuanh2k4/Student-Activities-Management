using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentActivities.src.Dtos.Admins;
using StudentActivities.src.Models;

namespace StudentActivities.src.Mappers
{
    public static class AdminMapper
    {
        public static async Task<Admins> ToAdminsFromCreateDto(this CreateAdminDto createAdminDto, int userid)
        {
            return new Admins
            {
                FirstName = createAdminDto.FirstName,
                LastName = createAdminDto.LastName,
                PhoneNumber = createAdminDto.PhoneNumber,
                Birth = createAdminDto.Birth,
                Email = createAdminDto.Email,
                UserId = userid
            };
        }
    }
}