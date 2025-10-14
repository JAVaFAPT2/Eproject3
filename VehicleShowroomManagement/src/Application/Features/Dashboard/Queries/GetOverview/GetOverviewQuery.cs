namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetOverview
{
    public record GetOverviewQuery : IRequest<OverviewDto>;

    public class OverviewDto
    {
        public decimal Profit { get; set; }
        public int Employees { get; set; }
        public int CustomersPurchased { get; set; }
        public int CompletedOrders { get; set; }
        public int Level2Models { get; set; }
        public int Vehicles { get; set; }
    }
}


