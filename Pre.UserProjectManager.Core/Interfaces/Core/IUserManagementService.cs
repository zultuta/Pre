using Pre.UserProjectManager.Core.DTO;
using Pre.UserProjectManager.Core.Models.APIResponse;

namespace Pre.UserProjectManager.Core.Interfaces.Core
{
    public interface IUserManagementService
    {
        Task<GenericResponse<string>> UpdateUserAsync(UpdateUserRequest request, long userId);
        Task<GenericResponse<UserDetails>> GetUserDetailsAsync(long userId);
        Task<GenericResponse<string>> DeleteUserAsync(long userId);
    }
}
