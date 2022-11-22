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
    public class GetUserProjectWithNoTrackingAsyncShould
    {
        const string dbName = "GetUserByProjectNoTrackNameDB";
        ProjectManagerQuery query;
        DateTime now;
        void Setup()
        {
            DbHelper.ClearDB(dbName);
            ProjectManagerDbContext dbContext = DbHelper.InitContext(dbName);
            query = new ProjectManagerQuery(dbContext);

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



        [Fact(DisplayName = "Should get the right data")]
        public async Task GetRightDataIfExist()
        {
            // Given   
            Setup();

            //when
            var project = await query.GetUserProjectWithNoTrackingAsync(1, "Project1");


            //Then

            project.Id.ShouldBe(1);
            project.Name.ShouldBe("Project1");
            project.UserId.ShouldBe(1);
            project.DateCreated.ShouldBe(now.Date);
            project.TimeCreated.ShouldBe(now);
            project.LastUpdatedTime.ShouldBe(now);
        }

        [Fact(DisplayName = "Should get the right data")]
        public async Task GetRightDataIfExist2()
        {
            // Given   
            Setup();

            //when
            var project = await query.GetUserProjectWithNoTrackingAsync(1, 1);


            //Then

            project.Id.ShouldBe(1);
            project.Name.ShouldBe("Project1");
            project.UserId.ShouldBe(1);
            project.DateCreated.ShouldBe(now.Date);
            project.TimeCreated.ShouldBe(now);
            project.LastUpdatedTime.ShouldBe(now);
        }

    }
}
