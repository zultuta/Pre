using Moq;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pre.UserProjectManager.Test.Core.Unit.UserManagementServiceSpec
{

    public class DeleteUserAsyncShould
    {
        Mock<IProjectManagerCommand> _commandMock;
        Mock<IProjectManagerQuery> _queryMock;
        UserManagementService _userManagementService;

        void Setup()
        {
            _queryMock = new();
            _commandMock = new();
            _userManagementService = new(_commandMock.Object, _queryMock.Object);
        }


        [Fact(DisplayName = "Call DeleteUserAsync method.")]
        public async Task CallDeleteUsertAsync()
        {
            // Given 
            Setup();

            //When
            await _userManagementService.DeleteUserAsync(It.IsAny<long>());

            //Then
            _commandMock.Verify(x => x.DeleteUserAsync(It.IsAny<long>()), Times.Once);
        }

        [Fact(DisplayName = "Return right response and message if user deleted successfully.")]
        public async Task ReturnRightResponseIfUserDeletedSuccessfully()
        {
            //Given
            Setup();

            //When
            var result = await _userManagementService.DeleteUserAsync(It.IsAny<long>());

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.USER_DELETED);
        }

        [Fact(DisplayName = "Catch exception and return with right response when error occurs while deleting user ")]
        public async Task CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            //Given
            Setup();


            _commandMock.Setup(x => x.DeleteUserAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _userManagementService.DeleteUserAsync(It.IsAny<long>());

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(500);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.GENERAL_FAILURE);
        }

    }
}
