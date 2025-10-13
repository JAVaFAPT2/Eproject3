
namespace VehicleShowroomManagement.Application.Features.DocumentOutputs.Queries.GetDocumentOutputs
{
    /// <summary>
    /// Handler for getting document outputs with pagination
    /// </summary>
    public class GetDocumentOutputsQueryHandler(IRepository<DocumentOutput> documentOutputRepository) : IRequestHandler<GetDocumentOutputsQuery, DocumentOutputsResponse>
    {
        public async Task<DocumentOutputsResponse> Handle(GetDocumentOutputsQuery request, CancellationToken cancellationToken)
        {
            var queryable = documentOutputRepository.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.EntityType))
            {
                if (Enum.TryParse<EntityType>(request.EntityType, true, out var entityTypeEnum))
                {
                    queryable = queryable.Where(doc => doc.EntityType == entityTypeEnum);
                }
            }

            if (!string.IsNullOrEmpty(request.EntityId))
            {
                queryable = queryable.Where(doc => doc.EntityId == request.EntityId);
            }

            // Get total count
            var totalCount = await documentOutputRepository.CountAsync(queryable, cancellationToken);

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            var documentOutputs = await documentOutputRepository.GetPagedAsync(
                queryable,
                skip,
                request.PageSize,
                cancellationToken);

            // Map to DTOs
            var documentOutputDtos = documentOutputs.Select(doc => new DocumentOutputDto
            {
                Id = doc.Id,
                EntityType = doc.EntityType.ToString(),
                EntityId = doc.EntityId,
                FileType = doc.FileType.ToString(),
                FilePath = doc.FilePath,
                FileName = doc.FileName,
                CreatedAt = doc.CreatedAt,
                UpdatedAt = doc.UpdatedAt
            }).ToList();

            return new DocumentOutputsResponse
            {
                DocumentOutputs = documentOutputDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };
        }
    }
}
