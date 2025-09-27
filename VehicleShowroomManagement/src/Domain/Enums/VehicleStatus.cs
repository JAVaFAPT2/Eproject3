namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Vehicle status in the inventory
    /// </summary>
    public enum VehicleStatus
    {
        /// <summary>
        /// Available for sale
        /// </summary>
        Available = 1,
        
        /// <summary>
        /// Sold to customer
        /// </summary>
        Sold = 2,
        
        /// <summary>
        /// Reserved for specific customer
        /// </summary>
        Reserved = 3,
        
        /// <summary>
        /// In service/repair
        /// </summary>
        InService = 4,
        
        /// <summary>
        /// Damaged and not available
        /// </summary>
        Damaged = 5,
        
        /// <summary>
        /// Returned by customer
        /// </summary>
        Returned = 6
    }
}