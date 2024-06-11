using System;
using Microsoft.AspNetCore.Identity;

namespace TaskManagementMicroservice.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public DateTime CreateAt { get; set; }
    }
}

