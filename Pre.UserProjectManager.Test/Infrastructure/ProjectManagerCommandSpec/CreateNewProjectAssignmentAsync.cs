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
    public class CreateNewProjectAssignmentAsync
    {
        ProjectManagerCommand _command;
        const string dbName = "ProjectAssignmentAddDb";


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
            await _command.CreateNewProjectAssignmentAsync(
               new()
               {
                   Id = 1,
                   AssignedBy = 1,
                   AssigneeUserId = 1,
                   ProjectId = 1,
                   DateCreated = now.Date,
                   LastUpdatedTime = now,
                   TimeCreated = now
               });

            List<ProjectAssignment> projAssignments = await DbHelper.GetEntityListAsync<ProjectAssignment>(dbName);

            //Then 
            projAssignments[0].Id.ShouldBe(1);
            projAssignments[0].AssignedBy.ShouldBe(1);
            projAssignments[0].AssigneeUserId.ShouldBe(1);
            projAssignments[0].ProjectId.ShouldBe(1);
            projAssignments[0].DateCreated.ShouldBe(now.Date);
            projAssignments[0].TimeCreated.ShouldBe(now);
            projAssignments[0].LastUpdatedTime.ShouldBe(now);
        }

    }
}
