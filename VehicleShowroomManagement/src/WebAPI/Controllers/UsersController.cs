using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Commands;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Application.Queries;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for user management operations
    /// Supports role-based access control
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all users with optional filtering
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "HR,Admin")] // Only HR and Admin can view all users
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
            [FromQuery] string? searchTerm,
            [FromQuery] int? roleId,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetUsersQuery(searchTerm, roleId, isActive, pageNumber, pageSize);
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "HR,Admin")] // Only HR and Admin can view specific users
        public Task<ActionResult<UserDto>> GetUser(int id)
        {
            // This would typically be implemented with a GetUserByIdQuery
            // For now, returning NotImplemented
            return Task.FromResult<ActionResult<UserDto>>(NotFound());
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "HR,Admin")] // Only HR and Admin can create users
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserRequest request)
        {
            var command = new CreateUserCommand(
                request.Username,
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.RoleId);

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUser), new { id = result.UserId }, result);
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "HR,Admin")] // Only HR and Admin can update users
        public Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            // This would typically be implemented with an UpdateUserCommand
            // For now, returning NotImplemented
            return Task.FromResult<IActionResult>(StatusCode(501, "Not Implemented"));
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can delete users
        public Task<IActionResult> DeleteUser(int id)
        {
            // This would typically be implemented with a DeleteUserCommand
            // For now, returning NotImplemented
            return Task.FromResult<IActionResult>(StatusCode(501, "Not Implemented"));
        }

        /// <summary>
        /// Gets current user profile
        /// </summary>
        [HttpGet("profile")]
        [Authorize] // Any authenticated user can view their own profile
        public Task<ActionResult<UserDto>> GetProfile()
        {
            // This would typically get the current user from the authentication context
            // For now, returning NotImplemented
            return Task.FromResult<ActionResult<UserDto>>(StatusCode(501, "Not Implemented"));
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
        public int RoleId { get; set; }
    }

    /// <summary>
    /// Request model for updating a user
    /// </summary>
    public class UpdateUserRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}