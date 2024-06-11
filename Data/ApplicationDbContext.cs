using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementMicroservice.Models.Entities;
using Task = TaskManagementMicroservice.Models.Task;

namespace TaskManagementMicroservice.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Project { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<ProjectMember> ProjectMember { get; set; }
    }
}
