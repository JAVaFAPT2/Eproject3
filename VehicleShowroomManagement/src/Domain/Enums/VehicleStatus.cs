namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Vehicle status in the inventory
    /// </summary>
    public enum VehicleStatus
    {
        /// <summary>
        /// Vehicle is available for sale
        /// </summary>
        Available = 1,
        
        /// <summary>
        /// Vehicle is reserved for a customer
        /// </summary>
        Reserved = 2,
        
        /// <summary>
        /// Vehicle has been sold
        /// </summary>
        Sold = 3
    }
}
