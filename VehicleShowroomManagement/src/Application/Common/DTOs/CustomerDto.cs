namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Customer information for order operations
    /// </summary>
    public class CustomerInfo(string id = "", string firstName = "", string lastName = "", string email = "", string? phone = null)
    {
        public string Id { get; set; } = id;
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string Email { get; set; } = email;
        public string? Phone { get; set; } = phone;
    }
}
