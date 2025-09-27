namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Purchase order status
    /// </summary>
    public enum PurchaseOrderStatus
    {
        /// <summary>
        /// Order created but not approved
        /// </summary>
        Pending = 1,
        
        /// <summary>
        /// Order approved by admin
        /// </summary>
        Approved = 2,
        
        /// <summary>
        /// Order sent to supplier
        /// </summary>
        Sent = 3,
        
        /// <summary>
        /// Order received from supplier
        /// </summary>
        Received = 4,
        
        /// <summary>
        /// Order completed
        /// </summary>
        Completed = 5,
        
        /// <summary>
        /// Order cancelled
        /// </summary>
        Cancelled = 6
    }
}