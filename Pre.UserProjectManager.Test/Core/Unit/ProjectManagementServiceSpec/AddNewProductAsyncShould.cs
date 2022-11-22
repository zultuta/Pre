using Moq;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pre.UserProjectManager.Test.Core.Unit.ProjectManagementServiceSpec
{
    public class AddNewProductAsyncShould
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


        [Fact(DisplayName = "Call GetUserProjectWithNoTrackingAsync method.")]
        public async Task CallGetUserProjectWithNoTrackingAsync()
        {
            // Given 
            Setup();

            //When
            await _projectManagementService.AddNewProductAsync(
                new()
                {
                    Name = "product1"
                }, 1);

            //Then
            _queryMock.Verify(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact(DisplayName = "Return with right response and error message if project does not belong to user")]
        public async Task ReturnWithRightresponseIfProjecDoesntBelongToUser()
        {
            // Given 
            Setup();

            //When
            var result = await _projectManagementService.AddNewProductAsync(
                new()
                {
                    Name = "product1"
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_PRODUCT_ADDITION);
        }

        [Fact(DisplayName = "Return with right response and error message if unit is not valid")]
        public async Task ReturnWithRightresponseIfUnitNotValid()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
           .ReturnsAsync(new Project());

            //When
            var result = await _projectManagementService.AddNewProductAsync(
                new()
                {
                    Name = "product1",
                    Unit = "y"
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_UNIT);
        }

        [Fact(DisplayName = "Call CreateNewProductAsync method.")]
        public async Task CallCreateNewProductAsync()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
           .ReturnsAsync(new Project());

            //When
            await _projectManagementService.AddNewProductAsync(
                new()
                {
                    Name = "product1",
                    Unit = "g"
                }, 1);

            //Then
            _commandMock.Verify(x => x.CreateNewProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact(DisplayName = "return right response and message if product saved successfully.")]
        public async Task ReturnRightResponseIfProductSavedSuccessfully()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
           .ReturnsAsync(new Project());

            //When
            var result = await _projectManagementService.AddNewProductAsync(
                new()
                {
                    Name = "product1",
                    Unit = "g"
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(201);
            result.Data.ShouldNotBeNull();
            result.Message.ShouldBe(MESSAGES.PRODUCT_ADDED);
        }

        [Fact(DisplayName = "catch exception and return with right response when error occurs while saving product ")]
        public async void CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
           .ReturnsAsync(new Project());

            _commandMock.Setup(x => x.CreateNewProductAsync(It.IsAny<Product>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _projectManagementService.AddNewProductAsync(
                new()
                {
                    Name = "product1",
                    Unit = "g"
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(500);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.GENERAL_FAILURE);
        }

    }
}
