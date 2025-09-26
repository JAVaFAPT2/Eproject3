using System;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;

namespace VehicleShowroomManagement.Application.Commands
{
    /// <summary>
    /// Command for updating user profile
    /// </summary>
    public class UpdateProfileCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public UpdateProfileCommand(string userId, string? fullName, string? address, string? phone)
        {
            UserId = userId;
            FullName = fullName;
            Address = address;
            Phone = phone;
        }
    }

    /// <summary>
    /// Command for changing password
    /// </summary>
    public class ChangePasswordCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public ChangePasswordCommand(string userId, string oldPassword, string newPassword)
        {
            UserId = userId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }

    /// <summary>
    /// Command for updating a user (admin function)
    /// </summary>
    public class UpdateUserCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public decimal? Salary { get; set; }
        public string? RoleId { get; set; }
        public bool? IsActive { get; set; }

        public UpdateUserCommand(
            string userId,
            string? firstName = null,
            string? lastName = null,
            string? email = null,
            string? phone = null,
            decimal? salary = null,
            string? roleId = null,
            bool? isActive = null)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Salary = salary;
            RoleId = roleId;
            IsActive = isActive;
        }
    }

    /// <summary>
    /// Command for deleting a user (admin function)
    /// </summary>
    public class DeleteUserCommand : IRequest<Unit>
    {
        public string UserId { get; set; }

        public DeleteUserCommand(string userId)
        {
            UserId = userId;
        }
    }
}