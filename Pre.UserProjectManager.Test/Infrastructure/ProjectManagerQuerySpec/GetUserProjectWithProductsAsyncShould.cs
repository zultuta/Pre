using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Infrastructure.Data;
using Pre.UserProjectManager.Infrastructure.Data.DAL;
using Pre.UserProjectManager.Test.Helper;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pre.UserProjectManager.Test.Infrastructure.ProjectManagerQuerySpec
{
    public class GetUserProjectWithProductsAsyncShould
    {
        const string dbName = "GetUserByProjectWithProdNameDB";
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

            DbHelper.SeedDB(dbName, new Product()
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
        }



        [Fact(DisplayName = "Should get the right data")]
        public async Task GetRightDataIfExist()
        {
            // Given   
            Setup();

            //when
            var project = await query.GetUserProjectWithProductsAsync(1, 1,0, 100);


            //Then

            project.Id.ShouldBe(1);
            project.Name.ShouldBe("Project1");
            project.UserId.ShouldBe(1);
            project.DateCreated.ShouldBe(now.Date);
            project.TimeCreated.ShouldBe(now);
            project.LastUpdatedTime.ShouldBe(now);

            var products = project.Products.ToList();

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
