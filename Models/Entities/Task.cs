using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagementMicroservice.Models.Entities;

namespace TaskManagementMicroservice.Models
{
    public class Task
    {
        [Key]
        public int Task_id { get; set; }

        public int Project_Id { get; set; }

        public string? Description { get; set; }

        [StringLength(255)]
        public string? Status { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public string? AssignedTo { get; set; }

        public DateTime DueDate { get; set; }

        [Required]
        [StringLength(255)]
        public string? Title { get; set; }

        [ForeignKey("Project_Id")]
        public virtual Project Project { get; set; } 

        [ForeignKey("AssignedTo")]
        public virtual ApplicationUser? AssignedUser { get; set; }
    }
}