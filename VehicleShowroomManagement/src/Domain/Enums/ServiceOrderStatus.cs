namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Service order status
    /// </summary>
    public enum ServiceOrderStatus
    {
        /// <summary>
        /// Service is scheduled
        /// </summary>
        Scheduled = 1,
        
        /// <summary>
        /// Service is in progress
        /// </summary>
        InProgress = 2,
        
        /// <summary>
        /// Service has been completed
        /// </summary>
        Completed = 3,
        
        /// <summary>
        /// Service has been cancelled
        /// </summary>
        Cancelled = 4
    }
}
