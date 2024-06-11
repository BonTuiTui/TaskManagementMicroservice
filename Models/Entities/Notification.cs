using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementMicroservice.Models.Entities
{
	public class Notification
	{
        [Key]
        public int Notification_id { get; set; }
        public string User_id { get; set; } 
        public string Notification_text { get; set; } 
        public DateTime CreateAt { get; set; } 
        public bool IsRead { get; set; }

        [ForeignKey("User_id")]
        public virtual ApplicationUser User { get; set; }
    }
}

