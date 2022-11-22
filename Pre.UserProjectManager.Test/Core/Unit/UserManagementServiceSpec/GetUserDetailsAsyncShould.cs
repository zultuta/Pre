using Moq;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pre.UserProjectManager.Test.Core.Unit.UserManagementServiceSpec
{
    public class GetUserDetailsAsyncShould
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


        [Fact(DisplayName = "Call GetUserByIdWithNoTrackingAsync method.")]
        public async Task CallGetUserByIdWithNoTrackingAsync()
        {
            // Given 
            Setup();

            //When
            await _userManagementService.GetUserDetailsAsync(It.IsAny<long>());

            //Then
            _queryMock.Verify(x => x.GetUserByIdWithNoTrackingAsync(It.IsAny<long>()), Times.Once);
        }

        [Fact(DisplayName = "Return right response and message if user details fetched successfully.")]
        public async Task ReturnRightResponseIfUserfetchedSuccessfully()
        {
            // Given 
            Setup();

            var now = DateTime.Now;

            _queryMock.Setup(x => x.GetUserByIdWithNoTrackingAsync(It.IsAny<long>()))
                .ReturnsAsync(new User()
                {
                    Id = 1,
                    UserName = "james1",
                    Email = "james@gmail.com",
                    FirstName = "James",
                    LastName = "Plato",
                    Password = "12345677",
                    PasswordSalt = "123",
                    DateCreated = now,
                    LastUpdatedTime = now,
                    TimeCreated = now,
                    LastLoginDate = now
                });

            //When
            var result = await _userManagementService.GetUserDetailsAsync(It.IsAny<long>());

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);

            result.Data.ShouldNotBeNull();
            result.Data.Id.ShouldBe(1);
            result.Data.UserName.ShouldBe("james1");
            result.Data.Email.ShouldBe("james@gmail.com");
            result.Data.FirstName.ShouldBe("James");
            result.Data.LastName.ShouldBe("Plato");
            result.Data.TimeCreated.ShouldBe(now);
            result.Data.LastLoginDate.ShouldBe(now);
            result.Data.LastUpdatedTime.ShouldBe(now);

            result.Message.ShouldBe(MESSAGES.SUCCESS);
        }

        [Fact(DisplayName = "Catch exception and return with right response when error occurs while getting user ")]
        public async Task CatchExceptionWithRightResponseWhenErrorOccurs()
        {
            // Given 
            Setup();


            _commandMock.Setup(x => x.UpdateExistingUserAsync(It.IsAny<User>()))
                .ThrowsAsync(new Exception());

            //Then
            var result = await _userManagementService.GetUserDetailsAsync(It.IsAny<long>());

            //Then
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeFalse();
            result.StatusCode.ShouldBe(500);
            result.Data.ShouldBeNull();
            result.Message.ShouldBe(MESSAGES.GENERAL_FAILURE);
        }

    }
}
