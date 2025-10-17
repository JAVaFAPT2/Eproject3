namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.SearchLevel2Models
{
    public class SearchLevel2ModelsQueryHandler(IRepository<VehicleModel> modelRepository, IRepository<VehicleSpec> specRepository)
        : IRequestHandler<SearchLevel2ModelsQuery, SearchLevel2ModelsResult>
    {
        public async Task<SearchLevel2ModelsResult> Handle(SearchLevel2ModelsQuery request, CancellationToken cancellationToken)
        {
            // Base filter: Level 2 (variants)
            var models = await modelRepository.FindAsync(m => m.Level == 2 && (request.ParentModelNumber == null || m.ParentId == request.ParentModelNumber), cancellationToken);

            // Seats / FuelType via specs (assumes specs attached to level-2 model via vehicleId field holding model number)
            if (request.Seats.HasValue || !string.IsNullOrWhiteSpace(request.FuelType))
            {
                var filtered = new List<VehicleModel>();
                foreach (var m in models)
                {
                    var specs = await specRepository.FindAsync(s => s.ModelId == m.ModelNumber, cancellationToken);
                    var seatsOk = !request.Seats.HasValue || specs.Any(s => s.SpecName == "Seats" && int.TryParse(s.SpecValue, out var v) && v == request.Seats.Value);
                    var fuelOk = string.IsNullOrWhiteSpace(request.FuelType) || specs.Any(s => s.SpecName == "Fuel Type" && string.Equals(s.SpecValue, request.FuelType, StringComparison.OrdinalIgnoreCase));
                    if (seatsOk && fuelOk) filtered.Add(m);
                }
                models = filtered;
            }

            var modelsList = models.ToList();
            var totalCount = modelsList.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            var paginatedModels = modelsList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new SearchLevel2ModelsResult
            {
                Items = paginatedModels,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };
        }
    }
}


