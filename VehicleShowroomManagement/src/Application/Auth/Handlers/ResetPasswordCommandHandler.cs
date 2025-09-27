using MediatR;
using VehicleShowroomManagement.Application.Auth.Commands;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.Auth.Handlers
{
    /// <summary>
    /// Handler for reset password functionality (disabled for Employee entity)
    /// </summary>
    public class ResetPasswordCommandHandler(
        IRepository<Employee> employeeRepository,
        IPasswordService passwordService)
        : IRequestHandler<ResetPasswordCommand>
    {
        public Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Note: Employee entity doesn't have password reset functionality
            // This would need to be implemented differently or use external auth
            // For now, we'll just throw an exception
            throw new NotImplementedException("Password reset is not supported for Employee entities. Use external authentication instead.");
        }
    }
}
