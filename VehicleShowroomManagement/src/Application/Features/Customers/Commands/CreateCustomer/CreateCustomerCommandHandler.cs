using MediatR;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Application.Features.Customers.Commands.CreateCustomer
{
    /// <summary>
    /// Handler for creating a new customer
    /// </summary>
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, string>
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(
            IRepository<Customer> customerRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Create address if provided
            Address? address = null;
            if (!string.IsNullOrEmpty(request.Street))
            {
                address = new Address(
                    request.Street,
                    request.City ?? string.Empty,
                    request.State ?? string.Empty,
                    request.ZipCode ?? string.Empty);
            }

            // Create customer
            var customer = new Customer(
                request.CustomerId,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Phone,
                address,
                request.Cccd);

            // Add to repository
            var result = await _customerRepository.AddAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result.Id;
        }
    }
}