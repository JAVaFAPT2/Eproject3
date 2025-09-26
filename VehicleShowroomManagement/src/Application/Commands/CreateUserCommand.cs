using System;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;

namespace VehicleShowroomManagement.Application.Commands
{
    /// <summary>
    /// Command for creating a new user
    /// </summary>
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int RoleId { get; set; }

        public CreateUserCommand(string username, string email, string password, string firstName, string lastName, int roleId)
        {
            Username = username;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            RoleId = roleId;
        }
    }
}