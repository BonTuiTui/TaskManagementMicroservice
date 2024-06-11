using TaskManagementMicroservice.Models.Entities;
using TaskManagementMicroservice.Services.Interfaces;

namespace TaskManagementMicroservice.Services.Implementations
{
    public class ProjectFactory : IProjectFactory
    {
        public ProjectFactory() { }

        IProjects IProjectFactory.CreateProject(string User_id, string Name, string Description)
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(Name));

            if (string.IsNullOrEmpty(Description))
                throw new ArgumentNullException(nameof(Description));

            return new Project(User_id, Name, Description, DateTime.Today, DateTime.Today);
        }
    }
}