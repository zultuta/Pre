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
    public class DeleteProjectAsyncShould
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
            await _projectManagementService.DeleteProjectAsync(It.IsAny<long>(), It.IsAny<long>());

            //Then
            _queryMock.Verify(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact(DisplayName = "return right response and message if project not exist.")]
        public async Task ReturnRightResponseIfProjectNotExist()
        {
            // Given 
            Setup();

            //When
            var result = await _projectManagementService.DeleteProjectAsync(It.IsAny<long>(), It.IsAny<long>()); ;

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_PROJECT_DELETE);
        }

        [Fact(DisplayName = "Call DeleteProjectAsync method.")]
        public async Task CallDeleteProjectAsync()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            //When
            await _projectManagementService.DeleteProjectAsync(It.IsAny<long>(), It.IsAny<long>());

            //Then
            _commandMock.Verify(x => x.DeleteProjectAsync(It.IsAny<long>()), Times.Once);
        }


        [Fact(DisplayName = "Return right response and message if project deleted successfully.")]
        public async Task ReturnRightResponseIfProjectDeletedSuccessfully()
        {
            //Given
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());

            //When
            var result = await _projectManagementService.DeleteProjectAsync(It.IsAny<long>(), It.IsAny<long>());

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.PROJECT_DELETED);
        }

        [Fact(DisplayName = "Catch exception and return with right response when error occurs while deleting project ")]
        public async Task CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            //Given
            Setup();

            _queryMock.Setup(x => x.GetUserProjectWithNoTrackingAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(new Project());


            _commandMock.Setup(x => x.DeleteProjectAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _projectManagementService.DeleteProjectAsync(It.IsAny<long>(), It.IsAny<long>());

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(500);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.GENERAL_FAILURE);
        }

    }
}
