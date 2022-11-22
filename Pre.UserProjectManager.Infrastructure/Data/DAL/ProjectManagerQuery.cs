using Microsoft.EntityFrameworkCore;
using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Models.Filters;

namespace Pre.UserProjectManager.Infrastructure.Data.DAL
{
    public class ProjectManagerQuery : IProjectManagerQuery
    {
        readonly ProjectManagerDbContext _appDbContext;

        public ProjectManagerQuery(ProjectManagerDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<User> GetUserByIdWithNoTrackingAsync(long userId)
        {
            return await _appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<Project> GetUserProjectWithNoTrackingAsync(long userId, string projectName)
        {
            return await _appDbContext.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.Name == projectName);
        }

        public async Task<Project> GetUserProjectWithNoTrackingAsync(long projectId, long userId)
        {
            return await _appDbContext.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == projectId && x.UserId == userId);
        }

        public async Task<bool> IsUserAssignedToProject(long projectId, long assigneeUserId)
        {
            return await _appDbContext.ProjectAssignments.AsNoTracking().AnyAsync(x => x.ProjectId == projectId && x.AssigneeUserId == assigneeUserId);
        }

        public async Task<ProjectAssignment> GetProjectAssignmentWithNoTrackingAsync(long projectId, long assigneeUserId)
        {
            return await _appDbContext.ProjectAssignments.AsNoTracking().FirstOrDefaultAsync(x => x.ProjectId == projectId && x.AssigneeUserId == assigneeUserId);
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<Project> GetUserProjectWithProductsAsync(long projectId, long userId, decimal minCarbonFootPrint, decimal maxCarbonFootPrint)
        {
            return await _appDbContext.Projects.AsNoTracking()
                .Where(x =>  x.Id == projectId && x.UserId == userId)
                .Include(prod => prod.Products.Where(prod => prod.CarbonFootPrint >= minCarbonFootPrint && prod.CarbonFootPrint <= maxCarbonFootPrint))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Project>> GetAllAsignedProjectsAsync(long userId, DateTime minDateAssigned, DateTime maxDateAssigned)
        {
            var projectIds = await _appDbContext.ProjectAssignments.Where(x => x.AssigneeUserId == userId
            && EF.Functions.DateDiffDay(minDateAssigned, x.DateCreated) >= 0 && EF.Functions.DateDiffDay(x.DateCreated, maxDateAssigned) >= 0).
                Select(ab => ab.ProjectId).ToListAsync();

            return await _appDbContext.Projects.Where(x => projectIds.Contains(x.Id)).ToListAsync();
        }

    }
}
