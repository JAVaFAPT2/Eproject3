using MediatR;

namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Queries.GetBillingDocuments
{
    /// <summary>
    /// Query to get billing documents with pagination and filtering
    /// </summary>
    public record GetBillingDocumentsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Status = null,
        string? OrderId = null) : IRequest<BillingDocumentsResponse>;
}
