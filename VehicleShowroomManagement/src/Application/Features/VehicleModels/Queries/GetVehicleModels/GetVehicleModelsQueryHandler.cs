namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModels
{
    public class GetVehicleModelsQueryHandler(IRepository<VehicleModel> modelRepository) : IRequestHandler<GetVehicleModelsQuery, GetVehicleModelsResult>
    {

        public async Task<GetVehicleModelsResult> Handle(GetVehicleModelsQuery request, CancellationToken cancellationToken)
        {
            var allModels = await modelRepository.GetAllAsync(cancellationToken);
            var vehicleModelsList = allModels.ToList();

            var totalCount = vehicleModelsList.Count;
            var pagedModels = vehicleModelsList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new GetVehicleModelsResult
            {
                VehicleModels = [.. pagedModels.Select(vm => new VehicleModelDto
                {
                    ModelNumber = vm.ModelNumber,
                    Name = vm.Name,
                    Price = vm.Price
                })],
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
