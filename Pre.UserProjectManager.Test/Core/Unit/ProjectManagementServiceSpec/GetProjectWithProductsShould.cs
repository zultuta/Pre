using Moq;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Pre.UserProjectManager.Test.Core.Unit.ProjectManagementServiceSpec
{
    public class GetProjectWithProductsShould
    {
        Mock<IProjectManagerCommand> _commandMock;
        Mock<IProjectManagerQuery> _queryMock;
        ProjectManagementService _projectManagementService;

        void Setup()
        {
            _queryMock = new();
            _commandMock = new();
            _projectManagementService = new(_commandMock.Object, _queryMock.Object);
        }

        [Fact(DisplayName = "Return with right response and error message if carbon filter range is invalid")]
        public async Task ReturnWithRightresponseIfCarbonFilterInvalid()
        {
            // Given 
            Setup();

            //When
            var result = await _projectManagementService.GetProjectWithProducts(
                new()
                {
                    maxCarbonFootPrint = 1,
                    minCarbonFootPrint = 11,
                }, 1, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_CARBON_FILTER);
        }

        [Fact(DisplayName = "Call GetUserProjectWithProductsAsync method.")]
        public async Task CallGetUserProjectWithProductsAsync()
        {
            // Given 
            Setup();

            //When
            await _projectManagementService.GetProjectWithProducts(
                new()
                {
                    maxCarbonFootPrint = 11,
                    minCarbonFootPrint = 1,
                }, 1, 1);

            //Then
            _queryMock.Verify(x => x.GetUserProjectWithProductsAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact(DisplayName = "Return with right response and error message if project not found")]
        public async Task ReturnWithRightresponseIfProjecNotFound()
        {
            // Given 
            Setup();

            //When
            var result = await _projectManagementService.GetProjectWithProducts(
                new()
                {
                    maxCarbonFootPrint = 11,
                    minCarbonFootPrint = 1,
                }, 1, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.PROJECT_NOT_FOUND);
        }


        [Fact(DisplayName = "Return right response and message if user project with products fetched successfully.")]
        public async Task ReturnRightResponseIfProjectWithProductsfetchedSuccessfully()
        {
            // Given 
            Setup();

            var now = DateTime.Now;

            _queryMock.Setup(x => x.GetUserProjectWithProductsAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ReturnsAsync(new Project()
                {
                    Id = 1,
                    Name = "Project1",
                    DateCreated = now,
                    LastUpdatedTime = now,
                    TimeCreated = now,
                    UserId = 1,
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Id = 1,
                            Name = "Product1",
                            Unit = "g",
                            Weight = 11,
                            ProjectId = 1,  
                            CarbonFootPrint = 11,
                            CarbonFootPrintPerGram = 1,
                            DateCreated = now.Date,
                            LastUpdatedTime = now,
                            TimeCreated = now
                        }
                    }
                });

            //When
            var result = await _projectManagementService.GetProjectWithProducts(
                new()
                {
                    maxCarbonFootPrint = 11,
                    minCarbonFootPrint = 1,
                }, 1, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);

            result.Data.ShouldNotBeNull();
            result.Data.Id.ShouldBe(1);
            result.Data.Name.ShouldBe("Project1");
            result.Data.TimeCreated.ShouldBe(now);
            result.Data.Products[0].Id.ShouldBe(1);
            result.Data.Products[0].ProductName.ShouldBe("Product1");
            result.Data.Products[0].Unit.ShouldBe("g");
            result.Data.Products[0].Weight.ShouldBe(11);
            result.Data.Products[0].CarbonFootPrint.ShouldBe(11);
            result.Data.Products[0].CarbonFootPrintPerGram.ShouldBe(1);
            result.Data.Products[0].TimeCreated.ShouldBe(now);


            result.Message.ShouldBe(MESSAGES.SUCCESS);
        }

        [Fact(DisplayName = "Catch exception and return with right response when error occurs while getting project with products ")]
        public async Task CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithProductsAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _projectManagementService.GetProjectWithProducts(
                new()
                {
                    maxCarbonFootPrint = 11,
                    minCarbonFootPrint = 1,
                }, 1, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(500);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.GENERAL_FAILURE);
        }

    }
}
