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

    public class GetUserByNameAsyncShould
    {
        const string dbName = "GetUserByUserNameDB";
        ProjectManagerQuery query;
        DateTime now;
        void Setup()
        {
            DbHelper.ClearDB(dbName);
            ProjectManagerDbContext dbContext = DbHelper.InitContext(dbName);
            query = new ProjectManagerQuery(dbContext);

            now = DateTime.Now;
            DbHelper.SeedDB(dbName, new User()
            {
                Id = 1,
                UserName = "james1",
                Email = "james@gmail.com",
                FirstName = "James",
                LastName = "Plato",
                Password = "12345677",
                PasswordSalt = "123",
                DateCreated = now.Date,
                LastUpdatedTime = now,
                TimeCreated = now,
                LastLoginDate = now
            });
        }



        [Fact(DisplayName = "Should get the right data")]
        public async Task GetRightDataIfExist()
        {
            // Given   
            Setup();

            //when
            var user = await query.GetUserByUserNameAsync("james1");


            //Then

            user.Id.ShouldBe(1);
            user.UserName.ShouldBe("james1");
            user.Email.ShouldBe("james@gmail.com");
            user.FirstName.ShouldBe("James");
            user.LastName.ShouldBe("Plato");
            user.Password.ShouldBe("12345677");
            user.PasswordSalt.ShouldBe("123");
            user.DateCreated.ShouldBe(now.Date);
            user.TimeCreated.ShouldBe(now);
            user.LastLoginDate.ShouldBe(now);
            user.LastUpdatedTime.ShouldBe(now);
        }

    }
}
