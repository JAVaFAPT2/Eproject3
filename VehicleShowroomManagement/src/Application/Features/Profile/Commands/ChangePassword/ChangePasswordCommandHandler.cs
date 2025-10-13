using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Features.Profile.Commands.ChangePassword
{
    /// <summary>
    /// Handler for change password command (unified User schema)
    /// </summary>
    public class ChangePasswordCommandHandler(
        IRepository<User> userRepository,
        IPasswordService passwordService) : IRequestHandler<ChangePasswordCommand, bool>
    {
        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return false;
            }

            // Verify current password
            if (!passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                return false;
            }

            // Hash new password and update
            var newPasswordHash = passwordService.HashPassword(request.NewPassword);
            user.ChangePassword(newPasswordHash);
            await userRepository.UpdateAsync(user, cancellationToken);

            return true;
        }
    }
}
