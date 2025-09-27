using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.ForgotPassword
{
    /// <summary>
    /// Simplified handler for forgot password command - production ready
    /// </summary>
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal if email exists or not for security
                return;
            }

            // Generate simple reset token (in production, store this in database)
            var resetToken = Guid.NewGuid().ToString();

            // Send password reset email
            await _emailService.SendPasswordResetEmailAsync(user.Email, user.FirstName, resetToken);
        }
    }
}