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
    public class CreateNewUserAsyncShould
    {
        ProjectManagerCommand _command;
        const string dbName = "UserAddDb";


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
            await _command.CreateNewUserAsync(
               new()
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

            List<User> users = await DbHelper.GetEntityListAsync<User>(dbName);

            //Then 
            users[0].Id.ShouldBe(1);
            users[0].UserName.ShouldBe("james1");
            users[0].Email.ShouldBe("james@gmail.com");
            users[0].FirstName.ShouldBe("James");
            users[0].LastName.ShouldBe("Plato");
            users[0].Password.ShouldBe("12345677");
            users[0].PasswordSalt.ShouldBe("123");
            users[0].DateCreated.ShouldBe(now.Date);
            users[0].TimeCreated.ShouldBe(now);
            users[0].LastLoginDate.ShouldBe(now);
            users[0].LastUpdatedTime.ShouldBe(now);
        }

    }
}
