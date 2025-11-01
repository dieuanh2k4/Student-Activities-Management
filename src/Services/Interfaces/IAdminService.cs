using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Admins;
using StudentActivities.src.Models;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<Admins>> GetAllAdmin();
        Task<Admins> CreateAdmin([FromForm] CreateAdminDto createAdminDto, int userid);
    }
}