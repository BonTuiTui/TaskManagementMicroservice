using System;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagementMicroservice.Models.Entities;

namespace TaskManagementMicroservice.Models.DTOs
{
    public class TaskDto
    {
        public int Task_id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime DueDate { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; }

        [ForeignKey("AssignedTo")]
        public virtual ApplicationUser? AssignedUser { get; set; } 
    }
}