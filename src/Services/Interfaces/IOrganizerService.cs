using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Clubs;
using StudentActivities.src.Dtos.Events;
using StudentActivities.src.Dtos.Organizers;
using StudentActivities.src.Models;

namespace StudentActivities.src.Services.Interfaces
{
    public interface IOrganizerService
    {
        Task<List<Organizers>> GetAllOrganizer();
        Task<Organizers> CreateOrganizer(CreateOrganizerDto createOrganizerDto, int userid);
        Task<Organizers> UpdateInforOrganizer([FromForm] UpdateOrganizerDto updateOrganizerDto, int id);

        // Task<bool> UpdateEventAsync(int organizerId, int eventId, UpdateEventDto dto);
        // Task<bool> UpdateClubAsync(int organizerId, int clubId, UpdateClubDto dto);
        // Task<OrganizerDashboardDto?> GetDashboardAsync(int organizerId);
        // Task<bool> DeleteEventAsync(int organizerId, int eventId);
    }
}