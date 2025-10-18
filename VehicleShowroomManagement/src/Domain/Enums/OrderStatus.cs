namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Order status in the sales process
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Order is pending processing
        /// </summary>
        Pending = 1,
        
        /// <summary>
        /// Order has been confirmed by dealer and vehicle assigned
        /// </summary>
        Confirmed = 2,
        
        /// <summary>
        /// Order has been completed
        /// </summary>
        Completed = 3,
        
        /// <summary>
        /// Order has been cancelled
        /// </summary>
        Cancelled = 4
    }
}
