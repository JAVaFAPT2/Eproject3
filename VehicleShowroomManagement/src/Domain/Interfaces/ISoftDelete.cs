using System;

namespace VehicleShowroomManagement.Domain.Interfaces
{
    /// <summary>
    /// Interface for entities that support soft delete
    /// </summary>
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}