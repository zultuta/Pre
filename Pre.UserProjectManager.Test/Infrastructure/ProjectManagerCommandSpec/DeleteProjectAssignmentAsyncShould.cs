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
    public class DeleteProjectAssignmentAsyncShould
    {
        ProjectManagerCommand _command;
        DateTime now;


        void Setup(string dbName)
        {
            DbHelper.ClearDB(dbName);
            ProjectManagerDbContext dbContext = DbHelper.InitContext(dbName);
            _command = new ProjectManagerCommand(dbContext);
            now = DateTime.Now;

            DbHelper.SeedDB(dbName, new ProjectAssignment()
            {
                AssignedBy = 1,
                AssigneeUserId = 1,
                ProjectId = 1,
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
            var dbName = "ProjectAssignmentDeleteDB";
            Setup(dbName);

            //when
            await _command.DeleteProjectAssignmentAsync(1);

            List<ProjectAssignment> projectAssignments = await DbHelper.GetEntityListAsync<ProjectAssignment>(dbName);

            //Then 
            projectAssignments.ShouldBeEmpty();
        }
    }
}
