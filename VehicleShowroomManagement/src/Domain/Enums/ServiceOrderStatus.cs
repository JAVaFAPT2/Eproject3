namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Service order status
    /// </summary>
    public enum ServiceOrderStatus
    {
        /// <summary>
        /// Service is in progress
        /// </summary>
        InProgress = 1,
        
        /// <summary>
        /// Service has been completed
        /// </summary>
        Completed = 2,
        
        /// <summary>
        /// Service has been cancelled
        /// </summary>
        Cancelled = 3
    }
}
