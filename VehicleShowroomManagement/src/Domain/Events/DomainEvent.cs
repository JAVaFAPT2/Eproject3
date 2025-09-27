using MediatR;

namespace VehicleShowroomManagement.Domain.Events
{
    /// <summary>
    /// Base class for all domain events
    /// Implements INotification for MediatR integration
    /// </summary>
    public abstract record DomainEvent : INotification
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string EventType { get; init; } = string.Empty;

        protected DomainEvent()
        {
            EventType = GetType().Name;
        }
    }
}