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
        public string RoleId { get; init; }

        public UserCreatedEvent(string userId, string username, string email, string roleId)
        {
            UserId = userId;
            Username = username;
            Email = email;
            RoleId = roleId;
        }
    }

    /// <summary>
    /// User profile updated domain event
    /// </summary>
    public record UserProfileUpdatedEvent : DomainEvent
    {
        public string UserId { get; init; }
        public string Name { get; init; }

        public UserProfileUpdatedEvent(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }

    /// <summary>
    /// User role changed domain event
    /// </summary>
    public record UserRoleChangedEvent : DomainEvent
    {
        public string UserId { get; init; }
        public string OldRoleId { get; init; }
        public string NewRoleId { get; init; }

        public UserRoleChangedEvent(string userId, string oldRoleId, string newRoleId)
        {
            UserId = userId;
            OldRoleId = oldRoleId;
            NewRoleId = newRoleId;
        }
    }
}
