using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.Users.Commands.UpdateUserProfile
{
    /// <summary>
    /// Handler for updating user profile
    /// </summary>
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateUserProfileCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new ArgumentException("User not found");

            user.UpdateProfile(request.FirstName, request.LastName, request.Phone);
            
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Publish domain event
            var profileUpdatedEvent = new UserProfileUpdatedEvent(user.Id, user.FirstName, user.LastName);
            await _mediator.Publish(profileUpdatedEvent, cancellationToken);
        }
    }
}