using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.DTO;
using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Interfaces.Core;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Models.APIResponse;
using Pre.UserProjectManager.Core.Utils;

namespace Pre.UserProjectManager.Core.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IProjectManagerCommand _projectManagerCommands;
        private readonly IProjectManagerQuery _projectManagerQueries;
        //private readonly ILoggerService loggerService;

        public UserManagementService(IProjectManagerCommand projectManagerCommands, IProjectManagerQuery projectManagerQueries)
        {
            _projectManagerCommands = projectManagerCommands;
            _projectManagerQueries = projectManagerQueries;
            //loggerService = _loggerService;
        }

        public async Task<GenericResponse<string>> UpdateUserAsync(UpdateUserRequest request, long userId)
        {
            try
            {
                var user = await _projectManagerQueries.GetUserByIdAsync(userId);

                if (!string.IsNullOrEmpty(request.FirstName))
                    user.FirstName = request.FirstName;

                if (!string.IsNullOrEmpty(request.LastName))
                    user.LastName = request.LastName;

                if (!string.IsNullOrEmpty(request.Email))
                    user.Email = request.Email;

                if (!string.IsNullOrEmpty(request.Password))
                    user.Password = PasswordHelper.HashPassword(request.Password, user.PasswordSalt);

                user.LastUpdatedTime = DateTime.Now;

                await _projectManagerCommands.UpdateExistingUserAsync(user);

                return GenericResponse<string>.Success(MESSAGES.USER_UPDATED);
            }
            catch(Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<string>.Error(MESSAGES.GENERAL_FAILURE);
            }
                  
        }

        public async Task<GenericResponse<UserDetails>> GetUserDetailsAsync(long userId)
        {
            try
            {
                var user = await _projectManagerQueries.GetUserByIdWithNoTrackingAsync(userId);

                return GenericResponse<UserDetails>.Success(MESSAGES.SUCCESS, new UserDetails()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Id = user.Id,
                    TimeCreated = user.TimeCreated,
                    LastLoginDate = user.LastLoginDate,
                    LastUpdatedTime = user.LastUpdatedTime
                });
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<UserDetails>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }

        public async Task<GenericResponse<string>> DeleteUserAsync(long userId)
        {
            try
            {
                await _projectManagerCommands.DeleteUserAsync(userId);

                return GenericResponse<string>.Success(MESSAGES.USER_DELETED);
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<string>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }
        
    }
}
