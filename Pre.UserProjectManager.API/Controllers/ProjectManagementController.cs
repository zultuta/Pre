using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.DTO;
using Pre.UserProjectManager.Core.Interfaces.Core;
using Pre.UserProjectManager.Core.Models.Filters;

namespace Pre.UserProjectManager.API.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/project")]
    [ApiController]
    public class ProjectManagementController : ControllerBase
    {
        private readonly IProjectManagementService _projectManagementService;
        public ProjectManagementController(IProjectManagementService projectManagementService)
        {
            _projectManagementService = projectManagementService;
        }

        /// <summary>
        /// Create a new project for logged in user.
        /// </summary>
        /// <response code="200">Returns newly created project</response>
        /// <response code="400">If project data is not valid</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("create")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddProjectAsync([FromBody] NewProjectRequest request)
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _projectManagementService.AddNewProjectAsync(request, userId);
            return StatusCode(response.StatusCode, response);
        }


        /// <summary>
        /// Assign user to project
        /// </summary>
        ///<response code="200">If project assigned successfully</response>
        /// <response code="400">If data is not valid</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("assign")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AssignProjectAsync([FromBody] ProjectAssignmentRequest request)
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _projectManagementService.AssignUserToProjectAsync(request, userId);
            return StatusCode(response.StatusCode, response);
        }


        /// <summary>
        /// Unassign user from project
        /// </summary>
        ///<response code="200">If user unassigned successfully</response>
        /// <response code="400">If data is not valid</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("unassign")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnAssignProjectAsync([FromBody] ProjectAssignmentRequest request)
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _projectManagementService.UnAssignUserFromProjectAsync(request, userId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Add product to project
        /// </summary>
        ///<response code="201">Returns newly created product</response>
        /// <response code="400">If product data is not valid</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("product/add")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddProductAsync([FromBody] NewProductRequest request)
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _projectManagementService.AddNewProductAsync(request, userId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Delete project
        /// </summary>
        ///<response code="200">If project deleted successfully</response>
        /// <response code="400">If projectId is not valid</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("delete/{projectId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteProjectAsync(long projectId)
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _projectManagementService.DeleteProjectAsync(projectId, userId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Update project
        /// </summary>
        ///<response code="200">If project updated successfully</response>
        /// <response code="400">If update data is not valid</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("edit")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProjectAsync([FromBody] UpdateProjectRequest request)
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _projectManagementService.UpdateProjectAsync(request, userId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get all user projects based on filter(if specified)
        /// </summary>
        /// <response code="200">Returned projects with the products</response>
        /// <response code="404">If project not found</response>
        /// <response code="400">If update data is not valid</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("get/{projectId}/product")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllProjectsProjectAsync(long projectId, [FromQuery] ProductFilter queryFilter)
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _projectManagementService.GetProjectWithProducts(queryFilter, projectId, userId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get all projects assigned to user based on filter(if specified). Date Filters are in format (MM-dd-yyyy)
        /// </summary>
        /// <response code="200">Returned assigned projects</response>
        /// <response code="400">If filter is not valid</response>
        /// <response code="401">If unathorized to perform this operation</response>
        [Route("getAll")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllAssignedProjectAsync([FromQuery] ProjectFilter queryFilter)
        {
            var userId = Convert.ToInt64(HttpContext.User.FindFirst(AppSettings.UserIdClaimType).Value);
            var response = await _projectManagementService.GetAssignedProjectsAsync(queryFilter, userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
