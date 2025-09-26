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
}