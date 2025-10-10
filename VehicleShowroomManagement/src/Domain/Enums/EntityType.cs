namespace VehicleShowroomManagement.Domain.Enums
{
    /// <summary>
    /// Entity type for document output reference
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// Document is for an Order
        /// </summary>
        Order = 1,
        
        /// <summary>
        /// Document is for a BillingDocument
        /// </summary>
        BillingDocument = 2,
        
        /// <summary>
        /// Document is for a PurchaseOrder
        /// </summary>
        PurchaseOrder = 3
    }
}

