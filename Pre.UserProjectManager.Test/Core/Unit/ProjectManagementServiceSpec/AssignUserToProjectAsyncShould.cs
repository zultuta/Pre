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
    public class AssignUserToProjectAsyncShould
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
            await _projectManagementService.AssignUserToProjectAsync(
                new()
                {
                    AssigneeUserId = 1
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
            var result = await _projectManagementService.AssignUserToProjectAsync(
                new()
                {
                    AssigneeUserId = 1
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_PROJECT_ASSIGNMENT);
        }

        [Fact(DisplayName = "Call GetUserByIdWithNoTrackingAsync method.")]
        public async Task CallGetUserByIdWithNoTrackingAsync()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            //When
            await _projectManagementService.AssignUserToProjectAsync(
                new()
                {
                    AssigneeUserId = 1
                }, 1);

            //Then
            _queryMock.Verify(x => x.GetUserByIdWithNoTrackingAsync(It.IsAny<long>()), Times.Once);
        }

        [Fact(DisplayName = "Return with right response and error message if assignee does not exist")]
        public async Task ReturnWithRightresponseIfAssigneeNotExist()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            //When
            var result = await _projectManagementService.AssignUserToProjectAsync(
                new()
                {
                    AssigneeUserId = 1
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_ASSIGNEE);
        }

        [Fact(DisplayName = "Call IsUserAssignedToProject method.")]
        public async Task CallIsUserAssignedToProject()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            _queryMock.Setup(x => x.GetUserByIdWithNoTrackingAsync(It.IsAny<long>()))
            .ReturnsAsync(new User());

            //When
            await _projectManagementService.AssignUserToProjectAsync(
                new()
                {
                    AssigneeUserId = 1
                }, 1);

            //Then
            _queryMock.Verify(x => x.IsUserAssignedToProject(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact(DisplayName = "Return with right response and error message if user already assigned to project")]
        public async Task ReturnWithRightresponseIfUserAlreadyAssigned()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            _queryMock.Setup(x => x.GetUserByIdWithNoTrackingAsync(It.IsAny<long>()))
            .ReturnsAsync(new User());

            _queryMock.Setup(x => x.IsUserAssignedToProject(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(true);

            //When
            var result = await _projectManagementService.AssignUserToProjectAsync(
                new()
                {
                    AssigneeUserId = 1
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.DUPLICATE_ASSIGNMENT);
        }

        [Fact(DisplayName = "Call CreateNewProjectAssignmentAsync method.")]
        public async Task CallCreateNewProjectAssignmentAsync() 
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            _queryMock.Setup(x => x.GetUserByIdWithNoTrackingAsync(It.IsAny<long>()))
            .ReturnsAsync(new User());

            _queryMock.Setup(x => x.IsUserAssignedToProject(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(false);

            //When
            var result = await _projectManagementService.AssignUserToProjectAsync(
                new()
                {
                    AssigneeUserId = 1
                }, 1);

            //Then
            _commandMock.Verify(x => x.CreateNewProjectAssignmentAsync(It.IsAny<ProjectAssignment>()), Times.Once);
        }

        [Fact(DisplayName = "return right response and message if project assigned successfully.")]
        public async Task ReturnRightResponseIfProjectAssignedSuccessfully()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            _queryMock.Setup(x => x.GetUserByIdWithNoTrackingAsync(It.IsAny<long>()))
            .ReturnsAsync(new User());

            _queryMock.Setup(x => x.IsUserAssignedToProject(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(false);

            //When
            var result = await _projectManagementService.AssignUserToProjectAsync(
                new()
                {
                    AssigneeUserId = 1
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            result.Message.ShouldBe(MESSAGES.PROJECT_ASSIGNED);
        }

        [Fact(DisplayName = "catch exception and return with right response when error occurs while assigning project")]
        public async void CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            _queryMock.Setup(x => x.GetUserByIdWithNoTrackingAsync(It.IsAny<long>()))
            .ReturnsAsync(new User());

            _queryMock.Setup(x => x.IsUserAssignedToProject(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(false);

            //When

            _commandMock.Setup(x => x.CreateNewProjectAssignmentAsync(It.IsAny<ProjectAssignment>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _projectManagementService.AssignUserToProjectAsync(
                new()
                {
                    AssigneeUserId = 1
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
