using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.DocumentOutputs.Commands.GenerateDocument
{
    public record GenerateDocumentCommand(
        EntityType EntityType,
        string EntityId,
        FileType FileType) : IRequest<string>;
}

