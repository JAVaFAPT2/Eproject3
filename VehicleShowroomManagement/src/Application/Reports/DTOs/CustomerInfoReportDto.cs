namespace VehicleShowroomManagement.Application.Reports.DTOs
{
    /// <summary>
    /// Data Transfer Object for Customer Information Report
    /// </summary>
    public class CustomerInfoReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public int TotalCustomers { get; set; }
        public int ActiveCustomers { get; set; }
        public int NewCustomersThisMonth { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<CustomerDetailDto> Customers { get; set; } = new List<CustomerDetailDto>();
        public List<CityCustomerDto> CustomersByCity { get; set; } = new List<CityCustomerDto>();
        public List<StateCustomerDto> CustomersByState { get; set; } = new List<StateCustomerDto>();
        public List<MonthlyCustomerDto> MonthlyCustomerTrends { get; set; } = new List<MonthlyCustomerDto>();
    }

    public class CustomerDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Cccd { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public List<CustomerOrderDto> Orders { get; set; } = new List<CustomerOrderDto>();
    }

    public class CustomerOrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string VehicleName { get; set; } = string.Empty;
        public string VehicleBrand { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }

    public class CityCustomerDto
    {
        public string City { get; set; } = string.Empty;
        public int CustomerCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    public class StateCustomerDto
    {
        public string State { get; set; } = string.Empty;
        public int CustomerCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    public class MonthlyCustomerDto
    {
        public string Month { get; set; } = string.Empty;
        public int NewCustomers { get; set; }
        public int TotalCustomers { get; set; }
        public decimal Revenue { get; set; }
    }
}
