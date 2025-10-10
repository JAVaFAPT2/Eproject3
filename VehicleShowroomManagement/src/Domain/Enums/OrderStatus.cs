namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Order status in the sales process
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Order is waiting for vehicle availability
        /// </summary>
        Waiting = 1,
        
        /// <summary>
        /// Vehicle has been reserved for this order
        /// </summary>
        Reserved = 2,
        
        /// <summary>
        /// Order has been confirmed by customer
        /// </summary>
        Confirmed = 3,
        
        /// <summary>
        /// Order has been completed
        /// </summary>
        Completed = 4,
        
        /// <summary>
        /// Order has been cancelled
        /// </summary>
        Cancelled = 5
    }
}
