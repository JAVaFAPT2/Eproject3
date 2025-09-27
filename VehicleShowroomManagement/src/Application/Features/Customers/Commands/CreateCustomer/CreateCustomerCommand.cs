using MediatR;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Application.Features.Customers.Commands.CreateCustomer
{
    /// <summary>
    /// Command to create a new customer
    /// </summary>
    public record CreateCustomerCommand : IRequest<string>
    {
        public string CustomerId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string? Email { get; init; }
        public string? Phone { get; init; }
        public string? Street { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
        public string? ZipCode { get; init; }
        public string? Cccd { get; init; }

        public CreateCustomerCommand(
            string customerId,
            string firstName,
            string lastName,
            string? email = null,
            string? phone = null,
            string? street = null,
            string? city = null,
            string? state = null,
            string? zipCode = null,
            string? cccd = null)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            Cccd = cccd;
        }
    }
}