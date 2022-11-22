using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Models.Filters;

namespace Pre.UserProjectManager.Core.Interfaces.Infrastructure
{
    public interface IProjectManagerQuery
    {
        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> GetUserByIdWithNoTrackingAsync(long userId);
        Task<User> GetUserByIdAsync(long userId);
        Task<Project> GetUserProjectWithNoTrackingAsync(long userId, string projectName);
        Task<Project> GetUserProjectWithNoTrackingAsync(long projectId, long userId);
        Task<bool> IsUserAssignedToProject(long projectId, long assigneeUserId);
        Task<ProjectAssignment> GetProjectAssignmentWithNoTrackingAsync(long projectId, long assigneeUserId);
        Task<Project> GetUserProjectWithProductsAsync(long projectId, long userId, decimal minCarbonFootPrint, decimal maxCarbonFootPrint);
        Task<IEnumerable<Project>> GetAllAsignedProjectsAsync(long userId, DateTime minDateCreated, DateTime maxDateCreated);
    }
}
