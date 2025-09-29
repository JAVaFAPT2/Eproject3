using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.WaitingLists.Queries.GetWaitingList
{
    /// <summary>
    /// Result for get waiting list query
    /// </summary>
    public class GetWaitingListResult
    {
        public List<WaitingListDto> WaitingListEntries { get; set; } = new List<WaitingListDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
