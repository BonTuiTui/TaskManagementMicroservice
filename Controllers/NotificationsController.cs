using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementMicroservice.Data;
using TaskManagementMicroservice.Models.Entities;

namespace TaskManagementMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("unreadcount/{user_id}")]
        public async Task<ActionResult<int>> GetUnreadNotificationsCount(string user_id)
        {
            if (user_id is null || user_id == "")
            {
                return BadRequest("ID of user is null or empty!");
            }
            else
            {
                var unreadCount = await _context.Notification
                    .Where(n => n.User_id == user_id && !n.IsRead)
                    .CountAsync();

                return Ok(unreadCount);
            }
        }

        [HttpGet("{user_id}")]
        public async Task<ActionResult> GetNotifications(string user_id)
        {
            List<Notification> notifications;

            if (user_id is null || user_id == "")
            {
                return BadRequest("ID of user is null or empty!");
            }
            else
            {
                notifications = await _context.Notification
                    .Where(n => n.User_id == user_id)
                    .OrderByDescending(n => n.CreateAt)
                    .ToListAsync();
                return Ok(notifications);
            }
        }

        [HttpPost("{notification_id}")]
        public async Task<ActionResult> MarkAsRead(int notification_id)
        {
            var notification = await _context.Notification.FindAsync(notification_id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
                return Ok("Notification with id " + notification_id + " is readed");
            }
            return NotFound("Not found notification with id " + notification_id);
        }
    }
}

