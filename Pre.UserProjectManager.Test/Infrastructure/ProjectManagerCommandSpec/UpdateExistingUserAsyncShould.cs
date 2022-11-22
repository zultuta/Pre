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
    public class UpdateExistingUserAsyncShould
    {
        ProjectManagerCommand _command;
        const string dbName = "UserUpdateDB";
        DateTime now;


        void Setup()
        {
            DbHelper.ClearDB(dbName);
            ProjectManagerDbContext dbContext = DbHelper.InitContext(dbName);
            _command = new ProjectManagerCommand(dbContext);
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
                LastLoginDate = now,
            });
        }

        [Fact(DisplayName = "Should update the right")]
        public async Task UpdateRightData()
        {
            // Given 
            Setup();

            //when
            await _command.UpdateExistingUserAsync(new User()
            {
                Id = 1,
                UserName = "james2",
                Email = "james@yahoo.com",
                FirstName = "Jamesy",
                LastName = "Mars",
                Password = "abcd12345",
                PasswordSalt = "wxyz"
            });

            List<User> users = await DbHelper.GetEntityListAsync<User>(dbName);

            //Then 
            users[0].Id.ShouldBe(1);
            users[0].UserName.ShouldBe("james2");
            users[0].Email.ShouldBe("james@yahoo.com");
            users[0].FirstName.ShouldBe("Jamesy");
            users[0].LastName.ShouldBe("Mars");
            users[0].Password.ShouldBe("abcd12345");
            users[0].PasswordSalt.ShouldBe("wxyz");
        }
    }
}
