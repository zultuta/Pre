
using Microsoft.AspNetCore.Mvc;
using Pre.UserProjectManager.Core.DTO;
using Pre.UserProjectManager.Core.Interfaces.Core;

namespace Pre.UserProjectManager.API.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Login to the system by authenticating using your username and password. New users can create an account using the create account endpoint.
        /// </summary>
        /// <response code="200">If user is autheticated successfully</response>
        /// <response code="401">If user authentication fails</response>
        [Route("login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AuthenticatAsync([FromBody] LoginRequest payload)
        {
            var response = await _authenticationService.AuthenticateUserAsync(payload);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Create an account as a new User.
        /// </summary>
        /// <response code="201">Returns newly created user</response>
        /// <response code="400">If user details are not valid</response>
        [Route("create")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserAsync([FromBody] NewUserRequest request)
        {
            var response = await _authenticationService.AddNewUserAsync(request);
            return StatusCode(response.StatusCode, response);
        }
    }
}
