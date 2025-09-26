using System;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;

namespace VehicleShowroomManagement.Application.Commands
{
    /// <summary>
    /// Command for user login
    /// </summary>
    public class LoginCommand : IRequest<LoginResultDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    /// <summary>
    /// Command for forgot password
    /// </summary>
    public class ForgotPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }

        public ForgotPasswordCommand(string email)
        {
            Email = email;
        }
    }

    /// <summary>
    /// Command for reset password
    /// </summary>
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }

        public ResetPasswordCommand(string token, string newPassword)
        {
            Token = token;
            NewPassword = newPassword;
        }
    }
}