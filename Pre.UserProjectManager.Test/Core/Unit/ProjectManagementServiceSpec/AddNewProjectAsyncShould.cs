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
    public class AddNewProjectAsyncShould
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
            await _projectManagementService.AddNewProjectAsync(
                new()
                {
                    ProjectName = "project1"
                }, 1);

            //Then
            _queryMock.Verify(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Return with right response and error message if project already exists")]
        public async Task ReturnWithRightresponseIfProjectAlreadyExist()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<string>()))
           .ReturnsAsync(new Project());

            //When
            var result = await _projectManagementService.AddNewProjectAsync(
                new()
                {
                    ProjectName = "project1"
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_PROJECT_NAME);
        }

        [Fact(DisplayName = "Call CreateNewProjectAsync method.")]
        public async Task CallCreateNewProjectAsync()
        {
            // Given 
            Setup();

            //When
            await _projectManagementService.AddNewProjectAsync(
                new()
                {
                    ProjectName = "project1"
                }, 1);

            //Then
            _commandMock.Verify(x => x.CreateNewProjectAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact(DisplayName = "return right response and message if project saved successfully.")]
        public async Task ReturnRightResponseIfProjectSavedSuccessfully()
        {
            // Given 
            Setup();

            //When
            var result = await _projectManagementService.AddNewProjectAsync(
                new()
                {
                    ProjectName = "project1"
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(201);
            result.Data.ShouldNotBeNull();
            result.Message.ShouldBe(MESSAGES.PROJECT_ADDED);
        }

        [Fact(DisplayName = "catch exception and return with right response when error occurs while saving project ")]
        public async Task CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            // Given 
            Setup();

            _commandMock.Setup(x => x.CreateNewProjectAsync(It.IsAny<Project>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _projectManagementService.AddNewProjectAsync(
                new()
                {
                    ProjectName = "project1"
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
