namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Goods receipt status
    /// </summary>
    public enum GoodsReceiptStatus
    {
        /// <summary>
        /// Receipt created but not processed
        /// </summary>
        Pending = 1,
        
        /// <summary>
        /// Goods accepted and processed
        /// </summary>
        Accepted = 2,
        
        /// <summary>
        /// Goods rejected due to issues
        /// </summary>
        Rejected = 3,
        
        /// <summary>
        /// Receipt completed
        /// </summary>
        Completed = 4
    }
}