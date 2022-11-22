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
    public class CreateNewProductAsyncShould
    {
        ProjectManagerCommand _command;
        const string dbName = "ProductAddDb";


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
            await _command.CreateNewProductAsync(
               new()
               {
                   Id = 1,
                   Name = "Product1",
                   Unit = "g",
                   Weight = 11,
                   CarbonFootPrint = 11,
                   CarbonFootPrintPerGram = 1,
                   ProjectId = 1,
                   DateCreated = now.Date,
                   LastUpdatedTime = now,
                   TimeCreated = now,
               });

            List<Product> products = await DbHelper.GetEntityListAsync<Product>(dbName);

            //Then 
            products[0].Id.ShouldBe(1);
            products[0].Name.ShouldBe("Product1");
            products[0].Unit.ShouldBe("g");
            products[0].Weight.ShouldBe(11);
            products[0].CarbonFootPrint.ShouldBe(11);
            products[0].CarbonFootPrintPerGram.ShouldBe(1);
            products[0].ProjectId.ShouldBe(1);
            products[0].DateCreated.ShouldBe(now.Date);
            products[0].TimeCreated.ShouldBe(now);
            products[0].LastUpdatedTime.ShouldBe(now);
        }

    }
}
