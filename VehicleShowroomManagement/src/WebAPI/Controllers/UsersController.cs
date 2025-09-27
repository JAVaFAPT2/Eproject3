using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Users.Commands.CreateUser;
using VehicleShowroomManagement.Application.Features.Users.Commands.UpdateUserProfile;
using VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for user management operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

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
                request.FirstName,
                request.LastName,
                request.Role,
                request.Phone);

            var userId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetUser), new { id = userId }, 
                new { id = userId, message = "User created successfully" });
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "HR,Admin")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            var query = new GetUserByIdQuery(id);
            var user = await _mediator.Send(query);

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
                request.FirstName,
                request.LastName,
                request.Phone);

            await _mediator.Send(command);
            
            return Ok(new { message = "User profile updated successfully" });
        }
    }

    /// <summary>
    /// Request model for creating a user
    /// </summary>
    public class CreateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? Phone { get; set; }
    }

    /// <summary>
    /// Request model for updating user profile
    /// </summary>
    public class UpdateUserProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }
}