using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModels
{
    public class GetVehicleModelsQueryHandler : IRequestHandler<GetVehicleModelsQuery, GetVehicleModelsResult>
    {
        private readonly IRepository<VehicleModel> _modelRepository;

        public GetVehicleModelsQueryHandler(IRepository<VehicleModel> modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<GetVehicleModelsResult> Handle(GetVehicleModelsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<VehicleModel> vehicleModels;

            if (!string.IsNullOrEmpty(request.Brand))
            {
                vehicleModels = await _modelRepository.FindAsync(vm => vm.Brand == request.Brand);
            }
            else
            {
                vehicleModels = await _modelRepository.GetAllAsync();
            }

            var totalCount = vehicleModels.Count();
            var pagedModels = vehicleModels
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new GetVehicleModelsResult
            {
                VehicleModels = pagedModels.Select(vm => new VehicleModelDto
                {
                    ModelNumber = vm.ModelNumber,
                    Name = vm.Name,
                    Brand = vm.Brand,
                    Price = vm.Price
                }).ToList(),
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
