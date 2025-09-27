namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Service order status
    /// </summary>
    public enum ServiceOrderStatus
    {
        /// <summary>
        /// Service order created
        /// </summary>
        Pending = 1,
        
        /// <summary>
        /// Service work in progress
        /// </summary>
        InProgress = 2,
        
        /// <summary>
        /// Service completed
        /// </summary>
        Completed = 3,
        
        /// <summary>
        /// Service cancelled
        /// </summary>
        Cancelled = 4
    }
}