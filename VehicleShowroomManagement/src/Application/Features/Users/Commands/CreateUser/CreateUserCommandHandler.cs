using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;
using VehicleShowroomManagement.Domain.ValueObjects;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Handler for creating a new user
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate email format
            var email = new Email(request.Email);
            var username = new Username(request.Username);

            // Hash password
            var passwordHash = _passwordService.HashPassword(request.Password);

            // Create user
            var user = new User(
                username.Value,
                email,
                passwordHash,
                request.FirstName,
                request.LastName,
                request.Role,
                request.Phone);

            // Add to repository
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Publish domain event
            var userCreatedEvent = new UserCreatedEvent(user.Id, user.Username, user.Email, user.Role);
            await _mediator.Publish(userCreatedEvent, cancellationToken);

            return user.Id;
        }
    }
}