using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Users.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Users.Handlers
{
    /// <summary>
    /// Handler for getting employees with search and pagination
    /// </summary>
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<SalesOrder> _orderRepository;

        public GetEmployeesQueryHandler(
            IRepository<Employee> employeeRepository,
            IRepository<SalesOrder> orderRepository)
        {
            _employeeRepository = employeeRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var allEmployees = await _employeeRepository.GetAllAsync();
            var employees = allEmployees.Where(e => !e.IsDeleted && e.IsActive).ToList();

            var allOrders = await _orderRepository.GetAllAsync();
            var orders = allOrders.Where(o => !o.IsDeleted && o.Status == "Completed").ToList();

            // Apply search filter
            var filteredEmployees = employees.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                filteredEmployees = filteredEmployees.Where(e =>
                    e.Name.ToLower().Contains(searchTerm) ||
                    e.EmployeeId.ToLower().Contains(searchTerm) ||
                    e.Role.ToLower().Contains(searchTerm));
            }

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            var paginatedEmployees = filteredEmployees.Skip(skip).Take(request.PageSize);

            var employeeDtos = new List<EmployeeDto>();
            foreach (var employee in paginatedEmployees)
            {
                var employeeDto = await MapToDto(employee);
                employeeDtos.Add(employeeDto);
            }
            return employeeDtos;
        }

        private async Task<EmployeeDto> MapToDto(Employee employee)
        {
            var allOrders = await _orderRepository.GetAllAsync();
            var employeeOrders = allOrders.Where(o => o.EmployeeId == employee.Id && !o.IsDeleted).ToList();

            return new EmployeeDto
            {
                Id = employee.Id,
                EmployeeNumber = employee.EmployeeId,
                Name = employee.Name,
                Role = employee.Role,
                Position = employee.Position,
                HireDate = employee.HireDate,
                Status = employee.Status,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt,
                TotalSales = employeeOrders.Count,
                TotalRevenue = employeeOrders.Sum(o => o.SalePrice)
            };
        }
    }
}
