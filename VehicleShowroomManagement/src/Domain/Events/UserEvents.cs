using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Events
{
    /// <summary>
    /// User created domain event
    /// </summary>
    public record UserCreatedEvent : DomainEvent
    {
        public string UserId { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public UserRole Role { get; init; }

        public UserCreatedEvent(string userId, string username, string email, UserRole role)
        {
            UserId = userId;
            Username = username;
            Email = email;
            Role = role;
        }
    }

    /// <summary>
    /// User profile updated domain event
    /// </summary>
    public record UserProfileUpdatedEvent : DomainEvent
    {
        public string UserId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public UserProfileUpdatedEvent(string userId, string firstName, string lastName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
    }

    /// <summary>
    /// User role changed domain event
    /// </summary>
    public record UserRoleChangedEvent : DomainEvent
    {
        public string UserId { get; init; }
        public UserRole OldRole { get; init; }
        public UserRole NewRole { get; init; }

        public UserRoleChangedEvent(string userId, UserRole oldRole, UserRole newRole)
        {
            UserId = userId;
            OldRole = oldRole;
            NewRole = newRole;
        }
    }

    /// <summary>
    /// Waiting list created domain event
    /// </summary>
    public record WaitingListCreatedEvent : DomainEvent
    {
        public string WaitingListId { get; init; }
        public string WaitId { get; init; }
        public string CustomerId { get; init; }
        public string ModelNumber { get; init; }

        public WaitingListCreatedEvent(string waitingListId, string waitId)
        {
            WaitingListId = waitingListId;
            WaitId = waitId;
        }
    }
}