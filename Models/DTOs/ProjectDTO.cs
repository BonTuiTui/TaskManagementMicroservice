using System;
namespace TaskManagementMicroservice.Models.DTOs
{
    public class ProjectDto
    {
        public int Project_id { get; set; }
        public string User_id { get; set; }
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Name { get; set; }
        public ICollection<ProjectMemberDto> ProjectMembers { get; set; }
        public ICollection<TaskDto> Task { get; set; }
    }
}