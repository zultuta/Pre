using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Infrastructure.Data;
using Pre.UserProjectManager.Infrastructure.Data.DAL;
using Pre.UserProjectManager.Test.Helper;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Pre.UserProjectManager.Test.Infrastructure.ProjectManagerCommandSpec
{
    public class DeleteProjecAsyncShould
    {
        ProjectManagerCommand _command;
        DateTime now;


        void Setup(string dbName)
        {
            DbHelper.ClearDB(dbName);
            ProjectManagerDbContext dbContext = DbHelper.InitContext(dbName);
            _command = new ProjectManagerCommand(dbContext);
            now = DateTime.Now;

            DbHelper.SeedDB(dbName, new Project()
            {
                Name = "Project1",
                UserId = 1,
                DateCreated = now.Date,
                LastUpdatedTime = now,
                TimeCreated = now,
                Id = 1,
            });
        }

        [Fact(DisplayName = "Should delete data")]
        public async Task DeleteRightData()
        {
            // Given 
            var dbName = "ProjectDeleteDB";
            Setup(dbName);

            //when
            await _command.DeleteProjectAsync(1);

            List<Project> projects = await DbHelper.GetEntityListAsync<Project>(dbName);

            //Then 
            projects.ShouldBeEmpty();
        }
    }
}
