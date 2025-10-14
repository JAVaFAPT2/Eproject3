using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Migrations.MigrateVehicleModelsV2;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/migrations")]
    [Authorize(Roles = "Admin")]
    public class MigrationsController(IMediator mediator) : ControllerBase
    {
        [HttpPost("vehicle-models-v2")]
        public async Task<IActionResult> RunVehicleModelsV2()
        {
            var result = await mediator.Send(new MigrateVehicleModelsV2Command());
            return Ok(new
            {
                message = "Vehicle models migration v2 completed",
                result.ModelsUpdated,
                result.Level1Assigned,
                result.Level2Assigned,
                result.SlugsGenerated
            });
        }
    }
}


