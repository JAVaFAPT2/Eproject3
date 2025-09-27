using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.WaitingLists.Queries.GetWaitingListById
{
    public class WaitingListDto
    {
        public string Id { get; set; } = string.Empty;
        public string WaitId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static WaitingListDto FromEntity(WaitingList waitingList)
        {
            return new WaitingListDto
            {
                Id = waitingList.Id,
                WaitId = waitingList.WaitId,
                CustomerId = waitingList.CustomerId,
                ModelNumber = waitingList.ModelNumber,
                RequestDate = waitingList.RequestDate,
                Status = waitingList.Status,
                CreatedAt = waitingList.CreatedAt,
                UpdatedAt = waitingList.UpdatedAt
            };
        }
    }
}