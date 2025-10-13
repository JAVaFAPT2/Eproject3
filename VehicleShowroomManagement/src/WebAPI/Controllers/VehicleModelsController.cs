using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.VehicleModels.Commands.CreateVehicleModel;
using VehicleShowroomManagement.Application.Features.VehicleModels.Commands.UpdateVehicleModel;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModels;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelById;

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
                request.Price,
                request.Description,
                request.ImageUrl);

            var result = await _mediator.Send(command);
            return Ok(new { modelNumber = result, message = "Vehicle model created successfully" });
        }

        /// <summary>
        /// Update vehicle model
        /// </summary>
        [HttpPut("{modelNumber}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateVehicleModel(string modelNumber, [FromBody] UpdateVehicleModelRequest request)
        {
            var command = new UpdateVehicleModelCommand(
                modelNumber,
                request.Name,
                request.Brand,
                request.Price,
                request.Description,
                request.ImageUrl);

            await _mediator.Send(command);
            return Ok(new { message = "Vehicle model updated successfully" });
        }

        /// <summary>
        /// Gets a vehicle model by model number
        /// </summary>
        [HttpGet("{modelNumber}")]
        public async Task<IActionResult> GetVehicleModel(string modelNumber)
        {
            var query = new GetVehicleModelByIdQuery(modelNumber);
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound(new { message = "Vehicle model not found" });
                
            return Ok(result);
        }

        /// <summary>
        /// Gets all vehicle models with pagination
        /// </summary>
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
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }

    public class UpdateVehicleModelRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}

