using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.VehicleModels.Commands.CreateVehicleModel;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModels;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehicleModelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehicleModelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateVehicleModel([FromBody] CreateVehicleModelRequest request)
        {
            var command = new CreateVehicleModelCommand(
                request.ModelNumber,
                request.Name,
                request.Brand,
                request.Price);

            var result = await _mediator.Send(command);
            return Ok(new { modelNumber = result, message = "Vehicle model created successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicleModels(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? brand = null)
        {
            var query = new GetVehicleModelsQuery(pageNumber, pageSize, brand);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }

    public class CreateVehicleModelRequest
    {
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}

