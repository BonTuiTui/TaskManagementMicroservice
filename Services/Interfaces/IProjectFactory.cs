namespace TaskManagementMicroservice.Services.Interfaces
{
    public interface IProjectFactory
    {
        IProjects CreateProject(string User_id, string Name, string Description);
    }
}