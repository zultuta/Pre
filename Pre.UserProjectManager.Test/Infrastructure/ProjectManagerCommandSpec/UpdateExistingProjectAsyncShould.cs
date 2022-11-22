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
    [Collection("NotThreadSafeResourceCollection")]
    public class UpdateExistingProjectAsyncShould
    {
        ProjectManagerCommand _command;
        const string dbName = "ProjectUpdateDB";
        DateTime now;


        void Setup()
        {
            DbHelper.ClearDB(dbName);
            ProjectManagerDbContext dbContext = DbHelper.InitContext(dbName);
            _command = new ProjectManagerCommand(dbContext);
            now = DateTime.Now;

            DbHelper.SeedDB(dbName, new Project()
            {
                Id = 1,
                Name = "Project1",
                UserId = 1,
                DateCreated = now.Date,
                LastUpdatedTime = now,
                TimeCreated = now,
            });
        }

        [Fact(DisplayName = "Should update the right")]
        public async Task UpdateRightData()
        {
            // Given 
            Setup();

            //when
            await _command.UpdateExistingProjectAsync(1, "Project2");

            List<Project> projects = await DbHelper.GetEntityListAsync<Project>(dbName);

            //Then 
            projects[0].Id.ShouldBe(1);
            projects[0].Name.ShouldBe("Project2");
            projects[0].UserId.ShouldBe(1);
            projects[0].DateCreated.ShouldBe(now.Date);
            projects[0].LastUpdatedTime.ShouldBeGreaterThan(now);
            projects[0].TimeCreated.ShouldBe(now);
        }
    }
}
