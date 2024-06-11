using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementMicroservice.Data;
using TaskManagementMicroservice.Models.Entities;

namespace TaskManagementMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MembersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("{project_id}")]
        public async Task<IActionResult> GetMembersOfProject(int project_id)
        {
            var project = await _context.Project.FindAsync(project_id);
            if (project is null)
            {
                return NotFound("Not found project with id " + project_id);
            }
            else
            {
                var projectMembers = await _context.ProjectMember
                    .Where(pm => pm.ProjectId == project_id)
                    .Select(pm => new
                    {
                        pm.User.Id,
                        pm.User.UserName,
                        pm.User.Email,
                        pm.User.FullName
                    })
                    .ToListAsync();

                return Ok(projectMembers);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMemberToProject(int projectId, string userName)
        {
            var project = await _context.Project.Include(p => p.ProjectMembers).FirstOrDefaultAsync(p => p.Project_id == projectId);
            if (project == null)
            {
                return NotFound("Not found project with id " + projectId);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user is null)
            {
                return NotFound("Not found user with username " + userName);
            }

            if (project.ProjectMembers.Any(pm => pm.UserId == user.Id))
            {
                return BadRequest("User is already a member of this project.");
            }

            project.ProjectMembers.Add(new ProjectMember { ProjectId = projectId, UserId = user.Id });
            await _context.SaveChangesAsync();

            return Ok(new { userName = user.UserName, email = user.Email });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveMember(int projectId, string userId)
        {
            var project = await _context.Project.Include(p => p.ProjectMembers).FirstOrDefaultAsync(p => p.Project_id == projectId);
            if (project == null)
            {
                return NotFound("Not found project with id " + projectId);
            }

            var projectMember = project.ProjectMembers.FirstOrDefault(pm => pm.UserId == userId);
            if (projectMember == null)
            {
                return NotFound("Not found member in projects with id " + userId);
            }

            var tasksAssignedToUser = await _context.Task
                .Where(t => t.Project_Id == projectId && t.AssignedTo == userId)
                .Select(t => new { t.Title })
                .ToListAsync();

            if (tasksAssignedToUser.Any())
            {
                var taskTitles = string.Join(", ", tasksAssignedToUser.Select(t => t.Title));
                return BadRequest($"Cannot remove user. The user is assigned to tasks: {taskTitles}");
            }

            _context.ProjectMember.Remove(projectMember);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}