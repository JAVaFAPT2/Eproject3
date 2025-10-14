namespace VehicleShowroomManagement.Application.Features.Users.Commands.UpdateUserActive
{
    /// <summary>
    /// Handler to update user's active status only
    /// </summary>
    public class UpdateUserActiveCommandHandler(IRepository<User> userRepository) : IRequestHandler<UpdateUserActiveCommand>
    {
        public async Task Handle(UpdateUserActiveCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken) 
                ?? throw new KeyNotFoundException($"User with ID {request.UserId} not found");

            if (request.IsActive) user.Activate(); else user.Deactivate();
            await userRepository.UpdateAsync(user, cancellationToken);
        }
    }
}


