namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Order status in the sales process
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Order created but not confirmed
        /// </summary>
        Pending = 1,
        
        /// <summary>
        /// Order confirmed by dealer
        /// </summary>
        Confirmed = 2,
        
        /// <summary>
        /// Order is being processed
        /// </summary>
        Processing = 3,
        
        /// <summary>
        /// Order completed successfully
        /// </summary>
        Completed = 4,
        
        /// <summary>
        /// Order cancelled
        /// </summary>
        Cancelled = 5,
        
        /// <summary>
        /// Order refunded
        /// </summary>
        Refunded = 6
    }
}