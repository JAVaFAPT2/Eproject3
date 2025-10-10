using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Users.Commands.UpdateUserProfile
{
    /// <summary>
    /// Handler for updating user profile (unified schema)
    /// </summary>
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
    {
        private readonly IRepository<User> _userRepository;

        public UpdateUserProfileCommandHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new ArgumentException("User not found");

            user.UpdateProfile(request.Name, request.Email, request.Phone, request.Address);
            
            await _userRepository.UpdateAsync(user);
        }
    }
}
