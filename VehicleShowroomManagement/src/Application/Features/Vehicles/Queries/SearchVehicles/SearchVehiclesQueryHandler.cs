namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.SearchVehicles
{
    /// <summary>
    /// Handler for searching vehicles - critical for showroom staff to find vehicles quickly
    /// </summary>
    public class SearchVehiclesQueryHandler(IRepository<Vehicle> vehicleRepository) : IRequestHandler<SearchVehiclesQuery, SearchVehiclesResult>
    {
        private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;

        public async Task<SearchVehiclesResult> Handle(SearchVehiclesQuery request, CancellationToken cancellationToken)
        {
            // Build filter criteria (no IsDeleted in new schema, uses DeletedAt)
            var allVehicles = await _vehicleRepository.GetAllAsync(cancellationToken);

            var filteredVehicles = allVehicles.Where(v =>
                (request.Status == null || v.Status == request.Status) &&
                (string.IsNullOrEmpty(request.ModelNumber) || v.ModelNumber.Contains(request.ModelNumber)) &&
                (string.IsNullOrEmpty(request.SearchTerm) || 
                 v.VehicleId.Contains(request.SearchTerm) || 
                 v.ModelNumber.Contains(request.SearchTerm)) &&
                (request.MinPrice == null || v.PurchasePrice >= request.MinPrice) &&
                (request.MaxPrice == null || v.PurchasePrice <= request.MaxPrice)).ToList();

            var totalCount = filteredVehicles.Count;
            
            // Apply pagination
            var pagedVehicles = filteredVehicles
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(v => new VehicleSearchDto
                {
                    VehicleId = v.VehicleId,
                    ModelNumber = v.ModelNumber,
                    ExternalNumber = v.ExternalNumber,
                    Status = v.Status,
                    PurchasePrice = v.PurchasePrice,
                    ReceiptDate = v.ReceiptDate,
                    IsAvailable = v.IsAvailable
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
