using Moq;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pre.UserProjectManager.Test.Core.Unit.ProjectManagementServiceSpec
{
    public class GetAssignedProjectsAsyncShould
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

        [Fact(DisplayName = "Return with right response and error message if filter date range is invalid")]
        public async Task ReturnWithRightresponseIfFilterDateInvalid()
        { 
            // Given 
            Setup();

            //When
            var result = await _projectManagementService.GetAssignedProjectsAsync(
                new()
                {
                    maxDateAssigned = DateTime.Now.AddDays(-1),
                    minDateAssigned = DateTime.Now,
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_DATE_ASSIGNED_FILTER);
        }

        [Fact(DisplayName = "Call GetAllAsignedProjectsAsync method.")]
        public async Task CallGetAllAsignedProjectsAsync()
        {
            // Given 
            Setup();

            //When
            await _projectManagementService.GetAssignedProjectsAsync(
                new()
                {
                    maxDateAssigned = DateTime.Now,
                    minDateAssigned = DateTime.Now.AddDays(-1),
                }, 1);

            //Then
            _queryMock.Verify(x => x.GetAllAsignedProjectsAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact(DisplayName = "Return right response and message if assigned projects fetched successfully.")]
        public async Task ReturnRightResponseIfAssignedProjectsfetchedSuccessfully()
        {
            // Given 
            Setup();

            //When
            var result = await _projectManagementService.GetAssignedProjectsAsync(
                new()
                {
                    maxDateAssigned = DateTime.Now,
                    minDateAssigned = DateTime.Now.AddDays(-1),
                }, 1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);

            result.Data.ShouldNotBeNull();
            result.Message.ShouldBe(MESSAGES.SUCCESS);
        }

        [Fact(DisplayName = "Catch exception and return with right response when error occurs while getting assigned projects")]
        public async Task CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetAllAsignedProjectsAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
           .ThrowsAsync(new Exception());

            //Then
            var result = await _projectManagementService.GetAssignedProjectsAsync(
                new()
                {
                    maxDateAssigned = DateTime.Now,
                    minDateAssigned = DateTime.Now.AddDays(-1),
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
