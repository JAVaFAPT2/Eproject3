using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Allotments.Queries.GetAllotmentById
{
    public class AllotmentDto
    {
        public string Id { get; set; } = string.Empty;
        public string AllotmentId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime AllotmentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static AllotmentDto FromEntity(Allotment allotment)
        {
            return new AllotmentDto
            {
                Id = allotment.Id,
                AllotmentId = allotment.AllotmentId,
                VehicleId = allotment.VehicleId,
                CustomerId = allotment.CustomerId,
                EmployeeId = allotment.EmployeeId,
                AllotmentDate = allotment.AllotmentDate,
                CreatedAt = allotment.CreatedAt,
                UpdatedAt = allotment.UpdatedAt
            };
        }
    }
}