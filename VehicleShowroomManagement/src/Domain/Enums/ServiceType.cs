namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Types of services provided
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// Pre-delivery inspection and preparation
        /// </summary>
        PreDelivery = 1,
        
        /// <summary>
        /// Regular maintenance service
        /// </summary>
        Maintenance = 2,
        
        /// <summary>
        /// Warranty repair service
        /// </summary>
        WarrantyRepair = 3,
        
        /// <summary>
        /// Customer requested repair
        /// </summary>
        CustomerRepair = 4,
        
        /// <summary>
        /// Vehicle detailing and cleaning
        /// </summary>
        Detailing = 5
    }
}