using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Commands.PrintOrder
{
    /// <summary>
    /// Handler for print order command
    /// </summary>
    public class PrintOrderCommandHandler : IRequestHandler<PrintOrderCommand, PrintOrderResult>
    {
        private readonly IRepository<SalesOrder> _salesOrderRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPdfService _pdfService;

        public PrintOrderCommandHandler(
            IRepository<SalesOrder> salesOrderRepository,
            IRepository<Customer> customerRepository,
            IVehicleRepository vehicleRepository,
            IUserRepository userRepository,
            IPdfService pdfService)
        {
            _salesOrderRepository = salesOrderRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
            _pdfService = pdfService;
        }

        public async Task<PrintOrderResult> Handle(PrintOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _salesOrderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new ArgumentException("Sales order not found");
            }

            // Get related entities
            var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
            var vehicle = await _vehicleRepository.GetByIdAsync(order.VehicleId);
            var salesPerson = await _userRepository.GetByIdAsync(order.SalesPersonId);

            // Generate PDF
            var pdfContent = await _pdfService.GenerateOrderPdfAsync(order, customer, vehicle, salesPerson);

            return new PrintOrderResult
            {
                Content = pdfContent,
                ContentType = "application/pdf",
                FileName = $"Order_{order.OrderNumber}_{DateTime.Now:yyyyMMdd}.pdf"
            };
        }
    }
}