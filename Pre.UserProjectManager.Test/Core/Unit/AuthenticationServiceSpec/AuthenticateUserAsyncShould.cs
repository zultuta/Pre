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

namespace Pre.UserProjectManager.Test.Core.Unit.AuthenticationServiceSpec
{
    public class AuthenticateUserAsyncShould
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
        public async Task CallGetUserByNameAsyncc()
        {
            // Given 
            Setup();

            //When
            await _authenticationService.AuthenticateUserAsync(new());

            //Then
            _queryMock.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Return with right response and error message if user not found")]
        public async Task ReturnWithRightresponseIfUserNotFound()
        {
            // Given 
            Setup();

            //When
            var result = await _authenticationService.AuthenticateUserAsync(new());

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(401);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_LOGIN);
        }

        [Fact(DisplayName = "Return with right response and error message if password incoreect")]
        public async Task ReturnWithRightresponseIfPasswordIsWrong()
        {
            // Given 
            Setup();

            var now = DateTime.Now;
            var salt = PasswordHelper.GenerateSalt();

            _queryMock.Setup(x => x.GetUserByUserNameAsync(It.IsAny<string>()))
           .ReturnsAsync(new User
           {
               Id = 1,
               UserName = "james1",
               Email = "james@gmail.com",
               FirstName = "James",
               LastName = "Plato",
               Password = "12345677",
               PasswordSalt = salt,
               DateCreated = now,
               LastUpdatedTime = now,
               TimeCreated = now,
               LastLoginDate = now
           });

            //When
            var result = await _authenticationService.AuthenticateUserAsync(new()
            {
                UserName = "james1",
                Password = "wrongPasssword"
            });

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(401);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.INVALID_LOGIN);
        }

        [Fact(DisplayName = "Call UpdateExistingUserAsync method.")]
        public async Task CallUpdateExistingUserAsync()
        {
            // Given 
            Setup();

            var now = DateTime.Now;
            var salt = PasswordHelper.GenerateSalt();

            _queryMock.Setup(x => x.GetUserByUserNameAsync(It.IsAny<string>()))
           .ReturnsAsync(new User
           {
               Id = 1,
               UserName = "james1",
               Email = "james@gmail.com",
               FirstName = "James",
               LastName = "Plato",
               Password = PasswordHelper.HashPassword("12345677", salt),
               PasswordSalt = salt,
               DateCreated = now,
               LastUpdatedTime = now,
               TimeCreated = now,
               LastLoginDate = now
           });

            //When
            await _authenticationService.AuthenticateUserAsync(new()
            {
                UserName = "james1",
                Password = "12345677"
            });

            //Then
            _commandMock.Verify(x => x.UpdateExistingUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "return right response and message if login succeeded.")]
        public async Task ReturnRightResponseIfLoginSucceeded()
        {
            // Given 
            Setup();

            var now = DateTime.Now;
            var salt = PasswordHelper.GenerateSalt();

            _queryMock.Setup(x => x.GetUserByUserNameAsync(It.IsAny<string>()))
           .ReturnsAsync(new User
           {
               Id = 1,
               UserName = "james1",
               Email = "james@gmail.com",
               FirstName = "James",
               LastName = "Plato",
               Password = PasswordHelper.HashPassword("12345677", salt),
               PasswordSalt = salt,
               DateCreated = now,
               LastUpdatedTime = now,
               TimeCreated = now,
               LastLoginDate = now
           });

            //When
            var result = await _authenticationService.AuthenticateUserAsync(new()
            {
                UserName = "james1",
                Password = "12345677"
            });

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            result.Message.ShouldBe(MESSAGES.LOGIN_SUCCESS);
        }

        [Fact(DisplayName = "catch exception and return with right response when error occurs while logging in")]
        public async void CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            // Given 
            Setup();

            var now = DateTime.Now;
            var salt = PasswordHelper.GenerateSalt();

            _queryMock.Setup(x => x.GetUserByUserNameAsync(It.IsAny<string>()))
           .ReturnsAsync(new User
           {
               Id = 1,
               UserName = "james1",
               Email = "james@gmail.com",
               FirstName = "James",
               LastName = "Plato",
               Password = PasswordHelper.HashPassword("12345677", salt),
               PasswordSalt = salt,
               DateCreated = now,
               LastUpdatedTime = now,
               TimeCreated = now,
               LastLoginDate = now
           });

            _commandMock.Setup(x => x.UpdateExistingUserAsync(It.IsAny<User>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _authenticationService.AuthenticateUserAsync(new()
            {
                UserName = "james1",
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
