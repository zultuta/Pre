using Pre.UserProjectManager.Core.DTO;
using Pre.UserProjectManager.Core.Models.APIResponse;

namespace Pre.UserProjectManager.Core.Interfaces.Core
{
    public interface IAuthenticationService
    {
        Task<GenericResponse<LoginResponse>> AuthenticateUserAsync(LoginRequest loginRequest);
        Task<GenericResponse<CreatedUser>> AddNewUserAsync(NewUserRequest newUser);
    }
}
