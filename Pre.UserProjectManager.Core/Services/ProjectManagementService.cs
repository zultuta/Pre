using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.DTO;
using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Enums;
using Pre.UserProjectManager.Core.Interfaces.Core;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Models.APIResponse;
using Pre.UserProjectManager.Core.Models.Filters;
using Pre.UserProjectManager.Core.Utils;
using System.Security.Cryptography;

namespace Pre.UserProjectManager.Core.Services
{
    public class ProjectManagementService : IProjectManagementService
    {
        private readonly IProjectManagerCommand _projectManagerCommands;
        private readonly IProjectManagerQuery _projectManagerQueries;
        //private readonly ILoggerService loggerService;

        public ProjectManagementService(IProjectManagerCommand projectManagerCommands, IProjectManagerQuery projectManagerQueries)
        {
            _projectManagerCommands = projectManagerCommands;
            _projectManagerQueries = projectManagerQueries;
            //loggerService = _loggerService;
        }

        public async Task<GenericResponse<CreatedProject>> AddNewProjectAsync(NewProjectRequest request, long userId)
        {
            try
            {
                var project = await _projectManagerQueries.GetUserProjectWithNoTrackingAsync(userId, request.ProjectName);

                if (project != null)
                    return GenericResponse<CreatedProject>.Fail(MESSAGES.INVALID_PROJECT_NAME);

                var now = DateTime.Now;
                Project newProject = new()
                {
                    Name = request.ProjectName,
                    UserId = userId,
                    DateCreated = now.Date,
                    TimeCreated = now,
                    LastUpdatedTime = now,
                };
                await _projectManagerCommands.CreateNewProjectAsync(newProject);

                return GenericResponse<CreatedProject>.Created(MESSAGES.PROJECT_ADDED, new()
                {
                    ProjectName = request.ProjectName,
                    Id = newProject.Id,
                    TimeCreated = newProject.TimeCreated
                });
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<CreatedProject>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }

        public async Task<GenericResponse<ProductDetail>> AddNewProductAsync(NewProductRequest request, long userId)
        {
            try
            {
                var project = await _projectManagerQueries.GetUserProjectWithNoTrackingAsync(request.ProjectId, userId);

                if (project == null)
                    return GenericResponse<ProductDetail>.Fail(MESSAGES.INVALID_PRODUCT_ADDITION);

                if (!Enum.IsDefined(typeof(UNITS), request.Unit))
                    return GenericResponse<ProductDetail>.Fail(MESSAGES.INVALID_UNIT);

                var now = DateTime.Now;
                Product newProduct = new()
                {
                    Name = request.Name,
                    ProjectId = request.ProjectId,
                    Weight = request.Weight,
                    Unit = request.Unit, 
                    CarbonFootPrintPerGram = request.CarbonFootPrintPerGram,
                    CarbonFootPrint = CarbonFootPrintCalculator.GetCarbonFootPrint(request.CarbonFootPrintPerGram, request.Weight, request.Unit),
                    DateCreated = now.Date,
                    TimeCreated = now,
                    LastUpdatedTime = now,
                };
                await _projectManagerCommands.CreateNewProductAsync(newProduct);

                return GenericResponse<ProductDetail>.Created(MESSAGES.PRODUCT_ADDED, new()
                {
                    ProductName = request.Name,
                    Unit = request.Unit,
                    Weight = request.Weight,
                    CarbonFootPrintPerGram = request.CarbonFootPrintPerGram,
                    CarbonFootPrint = newProduct.CarbonFootPrint,          
                    Id = newProduct.Id,
                    TimeCreated = newProduct.TimeCreated
                });
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<ProductDetail>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }

        public async Task<GenericResponse<CreatedAssignment>> AssignUserToProjectAsync(ProjectAssignmentRequest request, long userId)
        {
            try
            {
                var project = await _projectManagerQueries.GetUserProjectWithNoTrackingAsync(request.ProjectId, userId);

                if (project == null)
                    return GenericResponse<CreatedAssignment>.Fail(MESSAGES.INVALID_PROJECT_ASSIGNMENT);

                var user = await _projectManagerQueries.GetUserByIdWithNoTrackingAsync(request.AssigneeUserId);

                if (user == null)
                    return GenericResponse<CreatedAssignment>.Fail(MESSAGES.INVALID_ASSIGNEE);

                if (await _projectManagerQueries.IsUserAssignedToProject(request.ProjectId, request.AssigneeUserId))
                    return GenericResponse<CreatedAssignment>.Fail(MESSAGES.DUPLICATE_ASSIGNMENT);

                var now = DateTime.Now;
                ProjectAssignment newProjectAssignment = new()
                {
                    ProjectId = request.ProjectId,
                    AssigneeUserId = request.AssigneeUserId,
                    DateCreated = now.Date,
                    TimeCreated = now,
                    LastUpdatedTime = now,
                    AssignedBy = userId
                };
                await _projectManagerCommands.CreateNewProjectAssignmentAsync(newProjectAssignment);

                return GenericResponse<CreatedAssignment>.Success(MESSAGES.PROJECT_ASSIGNED, new CreatedAssignment
                {
                    AssigneeUserId = request.AssigneeUserId,
                    ProjectId = request.ProjectId,
                    Id = newProjectAssignment.Id,
                    TimeCreated = newProjectAssignment.TimeCreated,
                });
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<CreatedAssignment>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }

        public async Task<GenericResponse<string>> UnAssignUserFromProjectAsync(ProjectAssignmentRequest request, long userId)
        {
            try
            {
                var project = await _projectManagerQueries.GetUserProjectWithNoTrackingAsync(request.ProjectId, userId);

                if (project == null)
                    return GenericResponse<string>.Fail(MESSAGES.INVALID_PROJECT_UNASSIGNMENT);

                var projectAssignment = await _projectManagerQueries.GetProjectAssignmentWithNoTrackingAsync(request.ProjectId, request.AssigneeUserId);

                if (projectAssignment == null)
                    return GenericResponse<string>.Fail(MESSAGES.NON_EXISTING_ASSIGNMENT);

                await _projectManagerCommands.DeleteProjectAssignmentAsync(projectAssignment.Id);

                return GenericResponse<string>.Success(MESSAGES.PROJECT_UNASSIGNED);
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<string>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }

        public async Task<GenericResponse<string>> DeleteProjectAsync(long projectId, long userId)
        {
            try
            {
                var project = await _projectManagerQueries.GetUserProjectWithNoTrackingAsync(projectId, userId);

                if (project == null)
                    return GenericResponse<string>.Fail(MESSAGES.INVALID_PROJECT_DELETE);

                await _projectManagerCommands.DeleteProjectAsync(projectId);

                return GenericResponse<string>.Success(MESSAGES.PROJECT_DELETED);
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<string>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }

        public async Task<GenericResponse<string>> UpdateProjectAsync(UpdateProjectRequest request, long userId)
        {
            try
            {
                var project = await _projectManagerQueries.GetUserProjectWithNoTrackingAsync(request.ProjectId, userId);

                if (project == null)
                    return GenericResponse<string>.Fail(MESSAGES.INVALID_PROJECT_UPDATE);

                await _projectManagerCommands.UpdateExistingProjectAsync(request.ProjectId, request.ProjectName);

                return GenericResponse<string>.Success(MESSAGES.PROJECT_UPDATED);
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<string>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }

        public async Task<GenericResponse<ProjectWithProducts>> GetProjectWithProducts(ProductFilter filter, long projectId, long userId)
        {
            try
            {
                if (!filter.ValidCarbonFootPrinRange)
                    return GenericResponse<ProjectWithProducts>.Fail(MESSAGES.INVALID_CARBON_FILTER);

                var project = await _projectManagerQueries.GetUserProjectWithProductsAsync(projectId, userId, filter.minCarbonFootPrint, filter.maxCarbonFootPrint);
                
                if(project == null)
                    return GenericResponse<ProjectWithProducts>.NotFound(MESSAGES.PROJECT_NOT_FOUND);

                return GenericResponse<ProjectWithProducts>.Success(MESSAGES.SUCCESS, new ProjectWithProducts()
                {
                    Id = project.Id,
                    Name = project.Name,
                    TimeCreated = project.TimeCreated,
                    Products = project.Products.Select(p => new ProductDetail
                    {
                        ProductName = p.Name,
                        Id = p.Id,
                        Weight = p.Weight,
                        Unit = p.Unit,
                        CarbonFootPrintPerGram = p.CarbonFootPrintPerGram,
                        CarbonFootPrint = p.CarbonFootPrint,
                        TimeCreated = p.TimeCreated,
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<ProjectWithProducts>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }

        public async Task<GenericResponse<GroupedProjectDetails>> GetAssignedProjectsAsync(ProjectFilter filter, long userId)
        {
            try
            {
                if (!filter.ValidDateRange)
                    return GenericResponse<GroupedProjectDetails>.Fail(MESSAGES.INVALID_DATE_ASSIGNED_FILTER);

                var assignedProjects = await _projectManagerQueries.GetAllAsignedProjectsAsync(userId, filter.minDateAssigned, filter.maxDateAssigned);

                return GenericResponse<GroupedProjectDetails>.Success(MESSAGES.SUCCESS, new GroupedProjectDetails
                {
                    MyProjects = assignedProjects.Where(p => p.UserId == userId)
                    .Select(x => new ProjectDetail
                    {
                        Id = x.Id,
                        Name = x.Name,
                        TimeCreated = x.TimeCreated
                    }),

                    OtherProjects = assignedProjects.Where(p => p.UserId != userId)
                    .Select(x => new ProjectDetail
                    {
                        Id = x.Id,
                        Name = x.Name,
                        TimeCreated = x.TimeCreated
                    })
                });
            }
            catch(Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<GroupedProjectDetails>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }
    }
}
