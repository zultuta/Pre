using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Infrastructure.Data;
using Pre.UserProjectManager.Infrastructure.Data.DAL;
using Pre.UserProjectManager.Test.Helper;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pre.UserProjectManager.Test.Infrastructure.ProjectManagerQuerySpec
{
    public class GetProjectAssignmentWithNoTrackingAsyncShould
    {
        const string dbName = "GetProjAssignNoTrackNameDB";
        ProjectManagerQuery query;
        DateTime now;
        void Setup()
        {
            DbHelper.ClearDB(dbName);
            ProjectManagerDbContext dbContext = DbHelper.InitContext(dbName);
            query = new ProjectManagerQuery(dbContext);

            now = DateTime.Now;
            DbHelper.SeedDB(dbName, new ProjectAssignment()
            {
                Id = 1,
                AssignedBy = 1,
                AssigneeUserId = 1,
                ProjectId = 1,
                DateCreated = now.Date,
                LastUpdatedTime = now,
                TimeCreated = now
            });
        }



        [Fact(DisplayName = "Should get the right data")]
        public async Task GetRightDataIfExist()
        {
            // Given   
            Setup();

            //when
            var projAssignment = await query.GetProjectAssignmentWithNoTrackingAsync(1, 1);


            //Then

            projAssignment.Id.ShouldBe(1);
            projAssignment.AssignedBy.ShouldBe(1);
            projAssignment.AssigneeUserId.ShouldBe(1);
            projAssignment.ProjectId.ShouldBe(1);
            projAssignment.DateCreated.ShouldBe(now.Date);
            projAssignment.TimeCreated.ShouldBe(now);
            projAssignment.LastUpdatedTime.ShouldBe(now);
        }

    }
}
