using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Profile.Commands.UpdateProfile
{
    /// <summary>
    /// Handler for update profile command (unified User schema)
    /// </summary>
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
    {
        private readonly IRepository<User> _userRepository;

        public UpdateProfileCommandHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.UpdateProfile(request.Name, request.Email, request.Phone, request.Address);
            await _userRepository.UpdateAsync(user);
        }
    }
}
