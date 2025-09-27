using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.Login
{
    /// <summary>
    /// Command for user login authentication
    /// </summary>
    public record LoginCommand(string Username, string Password) : IRequest<LoginResultDto?>;
}