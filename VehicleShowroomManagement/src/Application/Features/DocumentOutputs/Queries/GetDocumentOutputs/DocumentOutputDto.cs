namespace VehicleShowroomManagement.Application.Features.DocumentOutputs.Queries.GetDocumentOutputs
{
    /// <summary>
    /// Data Transfer Object for Document Output
    /// </summary>
    public class DocumentOutputDto
    {
        public string Id { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Response model for paginated document outputs
    /// </summary>
    public class DocumentOutputsResponse
    {
        public List<DocumentOutputDto> DocumentOutputs { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
