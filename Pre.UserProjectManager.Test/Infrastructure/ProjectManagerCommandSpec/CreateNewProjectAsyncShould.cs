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
    public class CreateNewProjectAsyncShould
    {
        ProjectManagerCommand _command;
        const string dbName = "ProjectAddDb";


        void Setup()
        {
            DbHelper.ClearDB(dbName);
            ProjectManagerDbContext dbContext = DbHelper.InitContext(dbName);
            _command = new ProjectManagerCommand(dbContext);
        }

        [Fact(DisplayName = "Should save the right data")]
        public async Task SaveRightData()
        {
            // Given 
            Setup();
            var now = DateTime.Now;

            //when
            await _command.CreateNewProjectAsync(
               new()
               {
                   Id = 1,
                   Name = "Project1",
                   UserId = 1,
                   DateCreated = now.Date,
                   LastUpdatedTime = now,
                   TimeCreated = now,
               });

            List<Project> projects = await DbHelper.GetEntityListAsync<Project>(dbName);

            //Then 
            projects[0].Id.ShouldBe(1);
            projects[0].Name.ShouldBe("Project1");
            projects[0].UserId.ShouldBe(1);
            projects[0].DateCreated.ShouldBe(now.Date);
            projects[0].TimeCreated.ShouldBe(now);
            projects[0].LastUpdatedTime.ShouldBe(now);
        }

    }
}
