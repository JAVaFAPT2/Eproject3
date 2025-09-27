using MediatR;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.SearchVehicles
{
    /// <summary>
    /// Handler for searching vehicles - critical for showroom staff to find vehicles quickly
    /// </summary>
    public class SearchVehiclesQueryHandler : IRequestHandler<SearchVehiclesQuery, SearchVehiclesResult>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public SearchVehiclesQueryHandler(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<SearchVehiclesResult> Handle(SearchVehiclesQuery request, CancellationToken cancellationToken)
        {
            // Build filter criteria
            var vehicles = await _vehicleRepository.FindAsync(v => 
                !v.IsDeleted &&
                (request.Status == null || v.Status == request.Status) &&
                (string.IsNullOrEmpty(request.ModelNumber) || v.ModelNumber.Contains(request.ModelNumber)) &&
                (string.IsNullOrEmpty(request.SearchTerm) || 
                 v.VehicleId.Contains(request.SearchTerm) || 
                 v.ModelNumber.Contains(request.SearchTerm) ||
                 (!string.IsNullOrEmpty(v.Vin) && v.Vin.Contains(request.SearchTerm))) &&
                (request.MinPrice == null || v.PurchasePrice >= request.MinPrice) &&
                (request.MaxPrice == null || v.PurchasePrice <= request.MaxPrice),
                cancellationToken);

            var totalCount = vehicles.Count();
            
            // Apply pagination
            var pagedVehicles = vehicles
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(v => new VehicleSearchDto
                {
                    Id = v.Id,
                    VehicleId = v.VehicleId,
                    ModelNumber = v.ModelNumber,
                    Vin = v.Vin,
                    Status = v.Status,
                    PurchasePrice = v.PurchasePrice,
                    SalePrice = v.SalePrice,
                    ReceiptDate = v.ReceiptDate,
                    IsAvailable = v.IsAvailable,
                    IsRegistered = v.IsRegistered
                })
                .ToList();

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new SearchVehiclesResult
            {
                Vehicles = pagedVehicles,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                HasPreviousPage = request.PageNumber > 1,
                HasNextPage = request.PageNumber < totalPages
            };
        }
    }
}