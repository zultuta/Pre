using Pre.UserProjectManager.Core.Entity;

namespace Pre.UserProjectManager.Core.Interfaces.Infrastructure
{
    public interface IProjectManagerCommand
    {
        Task CreateNewUserAsync(User user);
        Task CreateNewProjectAsync(Project project);
        Task CreateNewProductAsync(Product product);
        Task CreateNewProjectAssignmentAsync(ProjectAssignment projectAssignment);
        Task DeleteProjectAssignmentAsync(long id);
        Task DeleteProjectAsync(long projecId);
        Task UpdateExistingProjectAsync(long projectId, string name);
        Task UpdateExistingUserAsync(User user);
        Task DeleteUserAsync(long userId);

    }
}
