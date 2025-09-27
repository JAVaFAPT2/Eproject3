using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Returns.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Returns.Handlers
{
    /// <summary>
    /// Handler for getting return requests with pagination
    /// </summary>
    public class GetReturnsQueryHandler : IRequestHandler<GetReturnsQuery, IEnumerable<ReturnRequestDto>>
    {
        private readonly IRepository<ReturnRequest> _returnRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public GetReturnsQueryHandler(
            IRepository<ReturnRequest> returnRepository,
            IRepository<Customer> customerRepository,
            IRepository<Vehicle> vehicleRepository)
        {
            _returnRepository = returnRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<ReturnRequestDto>> Handle(GetReturnsQuery request, CancellationToken cancellationToken)
        {
            var allReturns = await _returnRepository.GetAllAsync();
            var returns = allReturns.Where(r => !r.IsDeleted).ToList();

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            var paginatedReturns = returns.Skip(skip).Take(request.PageSize);

            var returnDtos = new List<ReturnRequestDto>();
            foreach (var returnRequest in paginatedReturns)
            {
                var returnDto = await MapToDto(returnRequest);
                returnDtos.Add(returnDto);
            }

            return returnDtos;
        }

        private async Task<ReturnRequestDto> MapToDto(ReturnRequest returnRequest)
        {
            var customer = await _customerRepository.GetByIdAsync(returnRequest.CustomerId);
            var vehicle = await _vehicleRepository.GetByIdAsync(returnRequest.VehicleId);

            return new ReturnRequestDto
            {
                Id = returnRequest.Id,
                OrderId = returnRequest.OrderId,
                CustomerId = returnRequest.CustomerId,
                Customer = customer != null ? new CustomerInfo
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone
                } : new CustomerInfo(),
                VehicleId = returnRequest.VehicleId,
                Vehicle = vehicle != null ? new VehicleInfo
                {
                    VehicleId = vehicle.Id,
                    VIN = vehicle.VehicleId,
                    ModelNumber = vehicle.ModelNumber,
                    Name = vehicle.ModelNumber,
                    Brand = "Unknown",
                    Price = vehicle.PurchasePrice
                } : new VehicleInfo(),
                Reason = returnRequest.Reason,
                Status = returnRequest.Status,
                Description = returnRequest.Description,
                RefundAmount = returnRequest.RefundAmount,
                RequestedAt = returnRequest.CreatedAt,
                ProcessedAt = returnRequest.ProcessedAt,
                ProcessedBy = returnRequest.ProcessedBy ?? string.Empty,
                Notes = returnRequest.Notes
            };
        }
    }
}
