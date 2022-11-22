using Pre.UserProjectManager.Core.DTO;
using Pre.UserProjectManager.Core.Models.APIResponse;
using Pre.UserProjectManager.Core.Models.Filters;

namespace Pre.UserProjectManager.Core.Interfaces.Core
{
    public interface IProjectManagementService
    {
        Task<GenericResponse<CreatedProject>> AddNewProjectAsync(NewProjectRequest request, long userId);
        Task<GenericResponse<ProductDetail>> AddNewProductAsync(NewProductRequest request, long userId);
        Task<GenericResponse<CreatedAssignment>> AssignUserToProjectAsync(ProjectAssignmentRequest request, long userId);
        Task<GenericResponse<string>> UnAssignUserFromProjectAsync(ProjectAssignmentRequest request, long userId);
        Task<GenericResponse<string>> DeleteProjectAsync(long projectId, long userId);
        Task<GenericResponse<string>> UpdateProjectAsync(UpdateProjectRequest request, long userId);
        Task<GenericResponse<ProjectWithProducts>> GetProjectWithProducts(ProductFilter filter, long projectId, long userId);
        Task<GenericResponse<GroupedProjectDetails>> GetAssignedProjectsAsync(ProjectFilter filter, long userId);
    }
}
