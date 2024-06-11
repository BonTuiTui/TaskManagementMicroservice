using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementMicroservice.Data;
using TaskManagementMicroservice.Models.DTOs;
using TaskManagementMicroservice.Models.Entities;
using TaskManagementMicroservice.Services.Interfaces;

namespace TaskManagementMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectFactory _projectFactory;

        public ProjectsController(ApplicationDbContext context, IProjectFactory projectFactory)
        {
            _context = context;
            _projectFactory = projectFactory;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetProjects([FromQuery] string userId, [FromQuery] string role)
        {
            List<Project> projects;

            if (role == "admin")
            {
                projects = _context.Project.ToList();
            }
            else if (role == "manager")
            {
                projects = _context.Project.Where(p => p.User_id == userId).ToList();
            }
            else
            {
                projects = _context.Project.Where(p => p.ProjectMembers.Any(pm => pm.UserId == userId)).ToList();
            }

            return Ok(projects);
        }

        [HttpGet("{project_id}")]
        public async Task<ActionResult> Detail(int project_id)
        {
            var project = await _context.Project
                .Include(p => p.Task)
                .ThenInclude(t => t.AssignedUser)
                .Include(p => p.ProjectMembers)
                .ThenInclude(pm => pm.User)
                .FirstOrDefaultAsync(p => p.Project_id == project_id);

            if (project == null)
            {
                return NotFound("Not found project with id " + project_id);
            }

            var projectDto = new ProjectDto
            {
                Project_id = project.Project_id,
                User_id = project.User_id,
                Description = project.Description,
                CreateAt = project.CreateAt,
                UpdateAt = project.UpdateAt,
                Name = project.Name,
                ProjectMembers = project.ProjectMembers.Select(pm => new ProjectMemberDto
                {
                    Id = pm.Id,
                    UserId = pm.UserId
                }).ToList(),
                Task = project.Task.Select(t => new TaskDto
                {
                    Task_id = t.Task_id,
                    Title = t.Title,
                    Description = t.Description,
                    CreateAt = t.CreateAt,
                    UpdateAt = t.UpdateAt,
                    DueDate = t.DueDate,
                    AssignedTo = t.AssignedTo,
                    AssignedUser = t.AssignedUser,
                    Status = t.Status
                }).ToList()
            };

            return Ok(projectDto);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateProject([FromBody] CreateProjectDto createProjectDto)
        {
            IProjects project =
                _projectFactory.CreateProject(createProjectDto.User_id,
                createProjectDto.Name, createProjectDto.Description);
            var _project = (Project)project;
            await _context.Project.AddAsync(_project);
            await _context.SaveChangesAsync();

            return Ok(_project.Project_id);
        }

        [HttpPut]
        public async Task<ActionResult<int>> UpdateProject([FromBody] Project receive_project)
        {
            var project = await _context.Project.FindAsync(receive_project.Project_id);

            if (project is null)
            {
                return NotFound("Not found project with id " + receive_project.Project_id + " to update!");
            }
            else
            {
                project.Name = receive_project.Name;
                project.Description = receive_project.Description;
                project.UpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(project.Project_id);
            }
        }

        [HttpDelete("{project_id}")]
        public async Task<IActionResult> Delete(int project_id)
        {
            var project = await _context.Project.FindAsync(project_id);

            if (project == null)
            {
                return NotFound("Not found project with id " + project_id); 
            }

            var relatedTasks = _context.Task.Where(t => t.Project_Id == project_id);
            _context.Task.RemoveRange(relatedTasks);

            var relatedProjectMembers = _context.ProjectMember.Where(pm => pm.ProjectId == project_id);
            _context.ProjectMember.RemoveRange(relatedProjectMembers);

            _context.Project.Remove(project);
            await _context.SaveChangesAsync();

            return Ok("Delete project with id " + project_id + " successful!");
        }

        
    }
}

