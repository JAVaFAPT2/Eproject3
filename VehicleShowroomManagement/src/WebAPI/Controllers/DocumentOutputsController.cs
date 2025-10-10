using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.DocumentOutputs.Commands.GenerateDocument;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Dealer,Admin")]
    public class DocumentOutputsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentOutputsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateDocument([FromBody] GenerateDocumentRequest request)
        {
            var command = new GenerateDocumentCommand(
                request.EntityType,
                request.EntityId,
                request.FileType);

            var documentId = await _mediator.Send(command);
            return Ok(new { id = documentId, message = "Document generated successfully" });
        }
    }

    public class GenerateDocumentRequest
    {
        public EntityType EntityType { get; set; }
        public string EntityId { get; set; } = string.Empty;
        public FileType FileType { get; set; }
    }
}

