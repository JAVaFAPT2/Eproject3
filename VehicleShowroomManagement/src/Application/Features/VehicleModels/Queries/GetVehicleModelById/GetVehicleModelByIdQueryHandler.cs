namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelById
{
    /// <summary>
    /// Handler for getting vehicle model by model number
    /// </summary>
    public class GetVehicleModelByIdQueryHandler(IRepository<VehicleModel> vehicleModelRepository) : IRequestHandler<GetVehicleModelByIdQuery, VehicleModelDto?>
    {
        public async Task<VehicleModelDto?> Handle(GetVehicleModelByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicleModel = await vehicleModelRepository.GetByIdAsync(request.ModelNumber, cancellationToken);

            if (vehicleModel == null)
                return null;

            return new VehicleModelDto
            {
                ModelNumber = vehicleModel.ModelNumber,
                Name = vehicleModel.Name,
                Price = vehicleModel.Price,
                Description = vehicleModel.Description,
                ImageUrl = string.Empty,
                Level = vehicleModel.Level,
                ParentModel = vehicleModel.ParentId
            };
        }
    }
}
