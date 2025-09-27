using MediatR;
using VehicleShowroomManagement.Application.Auth.Commands;
using VehicleShowroomManagement.Application.Email.Commands;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Auth.Handlers
{
    /// <summary>
    /// Handler for forgot password functionality (disabled for Employee entity)
    /// </summary>
    public class ForgotPasswordCommandHandler(
        IRepository<Employee> employeeRepository,
        IMediator mediator)
        : IRequestHandler<ForgotPasswordCommand>
    {
        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            // Find employee by employee ID
            var employee = await employeeRepository.FirstOrDefaultAsync(e =>
                e.EmployeeId == request.Email && // Using Email field for employee ID
                !e.IsDeleted &&
                e.IsActive);

            if (employee == null)
            {
                // For security, don't reveal if employee ID exists or not
                return;
            }

            // Note: Employee entity doesn't have password reset functionality
            // This would need to be implemented differently or use external auth
            // For now, we'll just return without doing anything
            return;
        }
    }
}
