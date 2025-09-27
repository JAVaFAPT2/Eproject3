namespace VehicleShowroomManagement.WebAPI.Models.Customers
{
    /// <summary>
    /// Request model for creating a customer
    /// </summary>
    public class CreateCustomerRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Cccd { get; set; }
    }
}