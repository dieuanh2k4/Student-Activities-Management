using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StudentActivities.src.Constant;

namespace StudentActivities.src.Dtos.Users
{
    public class CreateUserDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }

        [RegularExpression($"^({UserTypes.Admin}|{UserTypes.Student}|{UserTypes.Organizer})$", ErrorMessage = $"Quyền bắt buộc phải là '{UserTypes.Admin}', '{UserTypes.Student}', hoặc '{UserTypes.Organizer}'.")]
        public string? Role { get; set; }
    }
}