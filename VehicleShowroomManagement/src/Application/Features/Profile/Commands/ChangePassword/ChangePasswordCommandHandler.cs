using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Features.Profile.Commands.ChangePassword
{
    /// <summary>
    /// Handler for change password command (unified User schema)
    /// </summary>
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordService _passwordService;

        public ChangePasswordCommandHandler(
            IRepository<User> userRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return false;
            }

            // Verify current password
            if (!_passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                return false;
            }

            // Hash new password and update
            var newPasswordHash = _passwordService.HashPassword(request.NewPassword);
            user.ChangePassword(newPasswordHash);
            await _userRepository.UpdateAsync(user);

            return true;
        }
    }
}
