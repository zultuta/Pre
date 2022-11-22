using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.DTO;
using Pre.UserProjectManager.Core.Interfaces.Core;

namespace Pre.UserProjectManager.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        /// <summary>
        /// Update user details.
        /// </summary>
        /// <response code="200">If user details updated successfully</response>
        /// <response code="400">If update data is not valid</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("edit")]  
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRequest request)
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _userManagementService.UpdateUserAsync(request, userId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get details of logged in user.
        /// </summary>
        /// <response code="200">If user details retrieved</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("details")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _userManagementService.GetUserDetailsAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Delete logged in user.
        /// </summary>
        /// <response code="200">If user is deleted successfully</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("delete")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserAsync()
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _userManagementService.DeleteUserAsync(userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
