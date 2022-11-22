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

    public class UpdateProjectAsyncShould
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
            await _projectManagementService.UpdateProjectAsync(
                new()
                {
                    ProjectId = 1
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
            var result = await _projectManagementService.UpdateProjectAsync(
                new()
                {
                    ProjectId = 1
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_PROJECT_UPDATE);
        }

        [Fact(DisplayName = "Call UpdateExistingProjectAsync method.")]
        public async Task CallUpdateExistingProjectAsync()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            //When
            await _projectManagementService.UpdateProjectAsync(
                new()
                {
                    ProjectId = 1
                }, 1);

            //Then
            _commandMock.Verify(x => x.UpdateExistingProjectAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Return right response and message if project updated successfully.")]
        public async Task ReturnRightResponseIfProUpdatedSuccessfully()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            //When
            var result = await _projectManagementService.UpdateProjectAsync(
                new()
                {
                    ProjectId = 1
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.PROJECT_UPDATED);
        }

        [Fact(DisplayName = "Catch exception and return with right response when error occurs while updating project ")]
        public async Task CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            _commandMock.Setup(x => x.UpdateExistingProjectAsync(It.IsAny<long>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _projectManagementService.UpdateProjectAsync(
                new()
                {
                    ProjectId = 1
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
