namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Order item information
    /// </summary>
    public class OrderItemDto
    {
        public string ItemId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Discount { get; set; }
        public decimal LineTotal { get; set; }
    }
}


