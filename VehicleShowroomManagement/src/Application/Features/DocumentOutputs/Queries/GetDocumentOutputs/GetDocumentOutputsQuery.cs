using MediatR;

namespace VehicleShowroomManagement.Application.Features.DocumentOutputs.Queries.GetDocumentOutputs
{
    /// <summary>
    /// Query to get document outputs with pagination and filtering
    /// </summary>
    public record GetDocumentOutputsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? EntityType = null,
        string? EntityId = null) : IRequest<DocumentOutputsResponse>;
}
