using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Auth.Commands
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


}