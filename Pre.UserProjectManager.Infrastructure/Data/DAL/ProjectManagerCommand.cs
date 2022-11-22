using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;

namespace Pre.UserProjectManager.Infrastructure.Data.DAL
{
    public class ProjectManagerCommand : IProjectManagerCommand
    {
        readonly ProjectManagerDbContext _appDbContext;

        public ProjectManagerCommand(ProjectManagerDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task CreateNewUserAsync(User user)
        {
            _appDbContext.Add(user);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task CreateNewProjectAsync(Project project)
        {
            _appDbContext.Add(project);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task CreateNewProductAsync(Product product)
        {
            _appDbContext.Add(product);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task CreateNewProjectAssignmentAsync(ProjectAssignment projectAssignment)
        {
            _appDbContext.Add(projectAssignment);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteProjectAssignmentAsync(long id)
        {
            var existingProjectAssignment = new ProjectAssignment() { Id = id };
            _appDbContext.Remove(existingProjectAssignment);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(long projectId)
        {
            var existingProject = new Project() { Id = projectId };
            _appDbContext.Remove(existingProject);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateExistingProjectAsync(long projectId, string name)
        {
            var existingProject = new Project() { Id = projectId };
            _appDbContext.Attach(existingProject);

            existingProject.Name = name;
            existingProject.LastUpdatedTime = DateTime.Now;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateExistingUserAsync(User user)
        {
            _appDbContext.Update(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(long userId)
        {
            var existingUser = new User() { Id = userId };
            _appDbContext.Remove(existingUser);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
