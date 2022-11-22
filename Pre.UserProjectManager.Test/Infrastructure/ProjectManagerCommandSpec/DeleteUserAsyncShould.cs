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
    public class DeleteUserAsyncShould
    {
        ProjectManagerCommand _command;
        DateTime now;


        void Setup(string dbName)
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
                LastLoginDate = now
            });
        }

        [Fact(DisplayName = "Should delete data")]
        public async Task DeleteRightData()
        {
            // Given 
            var dbName = "UserDeleteDB";
            Setup(dbName);

            //when
            await _command.DeleteUserAsync(1);

            List<User> projects = await DbHelper.GetEntityListAsync<User>(dbName);

            //Then 
            projects.ShouldBeEmpty();
        }
    }
}
