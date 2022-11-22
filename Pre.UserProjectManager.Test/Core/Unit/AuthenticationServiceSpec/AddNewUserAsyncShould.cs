using Moq;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pre.UserProjectManager.Test.Core.Unit.AuthenticationServiceSpec
{
    public class AddNewUserAsyncShould
    {
        Mock<IProjectManagerCommand> _commandMock;
        Mock<IProjectManagerQuery> _queryMock;
        AuthenticationService _authenticationService;

        void Setup()
        {
            _queryMock = new();
            _commandMock = new();
            _authenticationService = new(_commandMock.Object, _queryMock.Object);
        }


        [Fact(DisplayName = "Call GetUserByNameAsync method.")]
        public async Task CallGetUserByNameAsync()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserByUserNameAsync(It.IsAny<string>()))
           .ReturnsAsync(It.IsAny<User>());

            //When
            await _authenticationService.AddNewUserAsync(
                new()
                {
                    FirstName = "James",
                });

            //Then
            _queryMock.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Return with right response and error message if user already exists")]
        public async Task ReturnWithRightresponseIfUserAlreadyExist()
        {
            // Given 
            Setup();

            _queryMock.Setup(x => x.GetUserByUserNameAsync(It.IsAny<string>()))
           .ReturnsAsync(new User());

            //When
            var result = await _authenticationService.AddNewUserAsync(
                new()
                {
                    FirstName = "James",
                });

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_USERNAME);
        }

        [Fact(DisplayName = "call CreateNewUserAsync method.")]
        public async Task CallCreateNewUserAsync()
        {
            // Given 
            Setup();

            _commandMock.Setup(x => x.CreateNewUserAsync(It.IsAny<User>())); ;

            //When
            await _authenticationService.AddNewUserAsync(
                new()
                {
                    FirstName = "James",
                    LastName = "Plato",
                    UserName = "user1",
                    Password = "12345677"
                });

            //Then
            _commandMock.Verify(x => x.CreateNewUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Return right response and message if user saved successfully.")]
        public async Task ReturnRightResponseIfUserSavedSuccessfully()
        {
            // Given 
            Setup();

            //When
            var result = await _authenticationService.AddNewUserAsync(
                new()
                {
                    FirstName = "James",
                    LastName = "Plato",
                    UserName = "user1",
                    Password = "12345677"
                });

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(201);
            result.Data.ShouldNotBeNull();
            result.Message.ShouldBe(MESSAGES.USER_ADDED);
        }

        [Fact(DisplayName = "Catch exception and return with right response when error occurs while saving user ")]
        public async Task CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            // Given 
            Setup();

            _commandMock.Setup(x => x.CreateNewUserAsync(It.IsAny<User>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _authenticationService.AddNewUserAsync(
                new()
                {
                    FirstName = "James",
                    LastName = "Plato",
                    UserName = "user1",
                    Password = "12345677"
                });

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(500);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.GENERAL_FAILURE);
        }

    }
}
