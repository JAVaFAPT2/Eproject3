namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.SearchVehicles
{
    /// <summary>
    /// Handler for searching vehicles - critical for showroom staff to find vehicles quickly
    /// </summary>
    public class SearchVehiclesQueryHandler(IRepository<Vehicle> vehicleRepository, IRepository<VehicleSpec> specRepository, IRepository<VehicleModel> modelRepository) : IRequestHandler<SearchVehiclesQuery, SearchVehiclesResult>
    {

        public async Task<SearchVehiclesResult> Handle(SearchVehiclesQuery request, CancellationToken cancellationToken)
        {
            // Build filter criteria
            var allVehicles = await vehicleRepository.GetAllAsync(cancellationToken);

            // If seats or fuelType provided, prefilter by model specs (level-2 model)
            HashSet<string>? allowedModelNumbers = null;
            if (request.Seats != null || !string.IsNullOrWhiteSpace(request.FuelType))
            {
                var allSpecs = await specRepository.GetAllAsync(cancellationToken);
                var specs = allSpecs.Where(s =>
                    (request.Seats == null || 
                     ((s.SpecName.Equals("Seats", StringComparison.OrdinalIgnoreCase) || 
                       s.SpecName.Equals("seats", StringComparison.OrdinalIgnoreCase)) && 
                      int.TryParse(s.SpecValue, out var val) && val == request.Seats)) ||
                    (!string.IsNullOrWhiteSpace(request.FuelType) && 
                     ((s.SpecName.Equals("Fuel Type", StringComparison.OrdinalIgnoreCase) || 
                       s.SpecName.Equals("fuelType", StringComparison.OrdinalIgnoreCase) ||
                       s.SpecName.Equals("fuel_type", StringComparison.OrdinalIgnoreCase)) && 
                      s.SpecValue.Equals(request.FuelType, StringComparison.OrdinalIgnoreCase)))
                );
                var modelIds = specs.Select(s => s.ModelId).ToHashSet();
                if (modelIds.Count > 0)
                {
                    var allModels = await modelRepository.GetAllAsync(cancellationToken);
                    var matchingModels = allModels.Where(m => modelIds.Contains(m.ModelNumber));
                    allowedModelNumbers = matchingModels.Select(m => m.ModelNumber).ToHashSet();
                }
                else
                {
                    // No models match specs -> return empty result early
                    return new SearchVehiclesResult { Items = new List<VehicleSearchDto>(), TotalCount = 0, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalPages = 0, HasPreviousPage = false, HasNextPage = false };
                }
            }

            var filteredVehicles = allVehicles.Where(v =>
                (request.Status == null || v.Status == request.Status) &&
                (string.IsNullOrEmpty(request.ModelNumber) || v.ModelNumber.Contains(request.ModelNumber)) &&
                (allowedModelNumbers == null || allowedModelNumbers.Contains(v.ModelNumber)) &&
                (string.IsNullOrEmpty(request.SearchTerm) || 
                 v.VehicleId.Contains(request.SearchTerm) || 
                 v.ModelNumber.Contains(request.SearchTerm) ||
                 (v.Vin != null && v.Vin.Contains(request.SearchTerm))) &&
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
                    IsAvailable = v.IsAvailable,
                    Vin = v.Vin
                })
                .ToList();

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new SearchVehiclesResult
            {
                Items = pagedVehicles,
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
