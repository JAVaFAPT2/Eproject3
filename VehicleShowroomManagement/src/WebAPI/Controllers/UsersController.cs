using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Users.Commands.CreateUser;
using VehicleShowroomManagement.Application.Features.Users.Commands.UpdateUserProfile;
using VehicleShowroomManagement.Application.Features.Users.Commands.UpdateUserActive;
using VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById;
using VehicleShowroomManagement.Application.Features.Users.Queries.GetUsersByRole;
using VehicleShowroomManagement.WebAPI.Models.Users;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for user management operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Creates a new user
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var command = new CreateUserCommand(
                request.Username,
                request.Email,
                request.Password,
                request.Name,
                request.RoleId,
                request.Phone,
                request.Address,
                request.HireDate);

            var userId = await mediator.Send(command);
            
            return CreatedAtAction(nameof(GetUser), new { id = userId }, 
                new { id = userId, message = "User created successfully" });
        }

        /// <summary>
        /// Gets users with optional roleName and searchTerm (phone/email)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<UserDto>>> GetUsers(
            [FromQuery] string? roleName = null,
            [FromQuery] string? searchTerm = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetUsersQuery(roleName, searchTerm, pageNumber, pageSize);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            var query = new GetUserByIdQuery(id);
            var user = await mediator.Send(query);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        /// <summary>
        /// Updates user profile
        /// </summary>
        [HttpPut("{id}/profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile(string id, [FromBody] UpdateUserProfileRequest request)
        {
            var command = new UpdateUserProfileCommand(
                id,
                request.Name,
                request.Email,
                request.Phone,
                request.Address);

            await mediator.Send(command);
            
            return Ok(new { message = "User profile updated successfully" });
        }

        /// <summary>
        /// Updates user's active status only
        /// </summary>
        [HttpPatch("{id}")]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> UpdateUserActive(string id, [FromBody] UpdateUserActiveRequest request)
        {
            var command = new UpdateUserActiveCommand(id, request.IsActive);
            await mediator.Send(command);

            return Ok(new { message = "User active status updated successfully" });
        }
    }
}
