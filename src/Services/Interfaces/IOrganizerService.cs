using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Organizers;
using StudentActivities.src.Models;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IOrganizerService
    {
        Task<List<Organizers>> GetAllOrganizer();
        Task<Organizers> CreateOrganizer(CreateOrganizerDto createOrganizerDto, int userid);
        Task<Organizers> UpdateInforOrganizer([FromForm] UpdateOrganizerDto updateOrganizerDto, int id);
    }
}