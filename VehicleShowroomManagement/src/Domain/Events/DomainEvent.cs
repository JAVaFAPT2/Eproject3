using System;
using MediatR;

namespace VehicleShowroomManagement.Domain.Events
{
    /// <summary>
    /// Base class for all domain events
    /// Implements INotification for MediatR integration
    /// </summary>
    public abstract class DomainEvent : INotification
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;

        protected DomainEvent()
        {
        }
    }
}