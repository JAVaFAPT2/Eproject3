namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModels
{
    public class GetVehicleModelsQueryHandler(IRepository<VehicleModel> modelRepository) : IRequestHandler<GetVehicleModelsQuery, GetVehicleModelsResult>
    {

        public async Task<GetVehicleModelsResult> Handle(GetVehicleModelsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<VehicleModel> vehicleModels;

            vehicleModels = !string.IsNullOrEmpty(request.Brand) ? (await modelRepository.FindAsync(vm => vm.Brand == request.Brand, cancellationToken)).ToList() : (await modelRepository.GetAllAsync(cancellationToken)).ToList();

            var totalCount = vehicleModels.Count();
            var pagedModels = vehicleModels
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new GetVehicleModelsResult
            {
                VehicleModels = [.. pagedModels.Select(vm => new VehicleModelDto
                {
                    ModelNumber = vm.ModelNumber,
                    Name = vm.Name,
                    Brand = vm.Brand,
                    Price = vm.Price
                })],
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
