using System;
namespace TaskManagementMicroservice.Models.DTOs
{
    public class CreateProjectDto
    {
        public string User_id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}