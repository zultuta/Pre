using Moq;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Services;
using Pre.UserProjectManager.Core.Utils;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pre.UserProjectManager.Test.Core.Unit.UserManagementServiceSpec
{
    public class UpdateUserAsyncShould
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


        [Fact(DisplayName = "Call GetUserByIdAsync method.")]
        public async Task CallGetUserByIdAsync()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<long>()))
           .ReturnsAsync(It.IsAny<User>());

            //When
            await _userManagementService.UpdateUserAsync(
                new()
                {
                    FirstName = "James",
                }, 1);

            //Then
            _queryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<long>()), Times.Once);
        }

        [Fact(DisplayName = "Call UpdateExistingUserAsync method.")]
        public async Task CallUpdateExistingUserAsync()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new User()
                {
                    Id = 1,
                    Email = "james@gmail.com",
                    FirstName = "James",
                    LastName = "Plato",
                    Password = "12345677",
                    PasswordSalt = PasswordHelper.GenerateSalt()
                });

            //When
            await _userManagementService.UpdateUserAsync(
                new()
                {
                    FirstName = "James",
                    LastName = "Plato",
                    Password = "12345677"
                }, 1);

            //Then
            _commandMock.Verify(x => x.UpdateExistingUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Return right response and message if user updated successfully.")]
        public async Task ReturnRightResponseIfUserUpdatedSuccessfully()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new User()
                {
                    Id = 1,
                    Email = "james@gmail.com",
                    FirstName = "James",
                    LastName = "Plato",
                    Password = "12345677",
                    PasswordSalt = PasswordHelper.GenerateSalt()
                });

            //When
            var result = await _userManagementService.UpdateUserAsync(
                new()
                {
                    FirstName = "James",
                    LastName = "Plato",
                    Password = "12345677"
                },1);

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.USER_UPDATED);
        }

        [Fact(DisplayName = "Catch exception and return with right response when error occurs while updating user ")]
        public async Task CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new User()
                {
                    Id = 1,
                    Email = "james@gmail.com",
                    FirstName = "James",
                    LastName = "Plato",
                    Password = "12345677",
                    PasswordSalt = PasswordHelper.GenerateSalt()
                });

            _commandMock.Setup(x => x.UpdateExistingUserAsync(It.IsAny<User>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _userManagementService.UpdateUserAsync(
                new()
                {
                    FirstName = "James",
                    LastName = "Plato",
                    Password = "12345677"
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
