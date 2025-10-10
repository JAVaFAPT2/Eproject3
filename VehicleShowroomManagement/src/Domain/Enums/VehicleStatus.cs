namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Vehicle status in the inventory
    /// </summary>
    public enum VehicleStatus
    {
        /// <summary>
        /// Vehicle is in stock and available for sale
        /// </summary>
        InStock = 1,
        
        /// <summary>
        /// Vehicle has been sold
        /// </summary>
        Sold = 2,
        
        /// <summary>
        /// Vehicle is reserved for a customer
        /// </summary>
        Reserved = 3,
        
        /// <summary>
        /// Vehicle is currently being serviced
        /// </summary>
        InService = 4
    }
}
