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
    public class IsUserAssignedToProjectShould
    {
        const string dbName = "IsUserAssignedDB";
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


        [Fact(DisplayName = "Should get the right response if user assigned")]
        public async Task GetRightDataIfExist()
        {
            // Given   
            Setup();

            //when
            var isAssigned = await query.IsUserAssignedToProject(1,1);


            //Then
            isAssigned.ShouldBeTrue();
        }


        [Fact(DisplayName = "Should get the right response if user not assigned")]
        public async Task GetRightResultIfDataDoesNotExist()
        {
            // Given   
            Setup();

            //when
            var isAssigned = await query.IsUserAssignedToProject(1, 2);


            //Then
            isAssigned.ShouldBeFalse();
        }


    }
}
