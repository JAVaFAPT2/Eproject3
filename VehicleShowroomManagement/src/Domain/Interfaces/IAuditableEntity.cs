using System;

namespace VehicleShowroomManagement.Domain.Interfaces
{
    /// <summary>
    /// Interface for entities that require audit trail
    /// </summary>
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}