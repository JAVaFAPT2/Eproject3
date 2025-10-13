
namespace VehicleShowroomManagement.Application.Features.Auth.Commands.ForgotPassword
{
    /// <summary>
    /// Handler for forgot password command
    /// </summary>
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(
            IRepository<User> userRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.FindAsync(u => u.Email == request.Email && u.DeletedAt == null);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                // Don't reveal if email exists or not for security
                return;
            }

            // Generate simple reset token (in production, store this in database)
            var resetToken = Guid.NewGuid().ToString();

            // Send password reset email
            await _emailService.SendPasswordResetEmailAsync(user.Email, user.Name ?? user.Username, resetToken);
        }
    }
}
