namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModels
{
    public class GetVehicleModelsQueryHandler(IRepository<VehicleModel> modelRepository) : IRequestHandler<GetVehicleModelsQuery, GetVehicleModelsResult>
    {

        public async Task<GetVehicleModelsResult> Handle(GetVehicleModelsQuery request, CancellationToken cancellationToken)
        {
            var allModels = await modelRepository.GetAllAsync(cancellationToken);
            var vehicleModelsList = allModels
                .Where(vm => vm.DeletedAt == null)
                .ToList();

            // Optional search filter by name, model number, or description
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim().ToLowerInvariant();
                vehicleModelsList = vehicleModelsList
                    .Where(vm =>
                        (vm.Name?.ToLowerInvariant().Contains(term) ?? false) ||
                        (vm.ModelNumber?.ToLowerInvariant().Contains(term) ?? false) ||
                        (vm.Description?.ToLowerInvariant().Contains(term) ?? false))
                    .ToList();
            }

            var totalCount = vehicleModelsList.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
            var pagedModels = vehicleModelsList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new GetVehicleModelsResult
            {
                Items = [.. pagedModels.Select(vm => new VehicleModelDto
                {
                    ModelNumber = vm.ModelNumber,
                    Name = vm.Name,
                    Price = vm.Price,
                    Level = vm.Level,
                    ParentModel = vm.ParentId,
                    Description = vm.Description
                })],
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };
        }
    }
}
