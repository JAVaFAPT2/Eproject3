namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Queries.GetBillingDocuments
{
    /// <summary>
    /// Handler for getting billing documents with pagination
    /// </summary>
    public class GetBillingDocumentsQueryHandler(IRepository<BillingDocument> billingDocumentRepository) : IRequestHandler<GetBillingDocumentsQuery, BillingDocumentsResponse>
    {

        public async Task<BillingDocumentsResponse> Handle(GetBillingDocumentsQuery request, CancellationToken cancellationToken)
        {
            var queryable = billingDocumentRepository.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.Status))
            {
                if (Enum.TryParse<BillingStatus>(request.Status, true, out var statusEnum))
                {
                    queryable = queryable.Where(bd => bd.Status == statusEnum);
                }
            }

            if (!string.IsNullOrEmpty(request.OrderId))
            {
                queryable = queryable.Where(bd => bd.OrderId == request.OrderId);
            }

            // Get total count
            var totalCount = await billingDocumentRepository.CountAsync(queryable, cancellationToken);

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
                var billingDocuments = await billingDocumentRepository.GetPagedAsync(
                queryable,
                skip,
                request.PageSize,
                cancellationToken);

            // Map to DTOs
            var billingDocumentDtos = billingDocuments.Select(bd => new BillingDocumentDto
            {
                Id = bd.Id,
                OrderId = bd.OrderId,
                CreatedBy = bd.CreatedBy,
                Amount = bd.Amount,
                AppointmentDate = bd.AppointmentDate,
                Status = bd.Status.ToString(),
                CreatedAt = bd.CreatedAt,
                UpdatedAt = bd.UpdatedAt,
                // Map computed properties from domain entity
                IsUnpaid = bd.IsUnpaid,
                IsPartiallyPaid = bd.IsPartiallyPaid,
                IsPaid = bd.IsPaid
            }).ToList();

            return new BillingDocumentsResponse
            {
                BillingDocuments = billingDocumentDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };
        }
    }
}
