namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Purchase order status
    /// </summary>
    public enum PurchaseOrderStatus
    {
        /// <summary>
        /// Purchase order is pending approval or delivery
        /// </summary>
        Pending = 1,
        
        /// <summary>
        /// Purchase order has been completed and vehicles created
        /// </summary>
        Completed = 2,
        
        /// <summary>
        /// Purchase order has been cancelled
        /// </summary>
        Cancelled = 3
    }
}
