using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Application.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Handlers
{
    /// <summary>
    /// Handler for getting employees with search and pagination
    /// </summary>
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<SalesOrder> _orderRepository;

        public GetEmployeesQueryHandler(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IRepository<SalesOrder> orderRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var allUsers = await _userRepository.GetAllAsync();
            var users = allUsers.Where(u => !u.IsDeleted && u.IsActive).ToList();

            var allRoles = await _roleRepository.GetAllAsync();
            var roles = allRoles.Where(r => !r.IsDeleted).ToList();

            var allOrders = await _orderRepository.GetAllAsync();
            var orders = allOrders.Where(o => !o.IsDeleted && o.Status == "COMPLETED").ToList();

            // Apply search filter
            var filteredUsers = users.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                filteredUsers = filteredUsers.Where(u =>
                    u.FirstName.ToLower().Contains(searchTerm) ||
                    u.LastName.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    u.Username.ToLower().Contains(searchTerm));
            }

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            var paginatedUsers = filteredUsers.Skip(skip).Take(request.PageSize);

            var employeeDtos = new List<EmployeeDto>();
            foreach (var user in paginatedUsers)
            {
                var employeeDto = await MapToDto(user);
                employeeDtos.Add(employeeDto);
            }
            return employeeDtos;
        }

        private async Task<EmployeeDto> MapToDto(User user)
        {
            var role = await _roleRepository.GetByIdAsync(user.RoleId);
            var allOrders = await _orderRepository.GetAllAsync();
            var userOrders = allOrders.Where(o => o.SalesPersonId == user.Id && !o.IsDeleted).ToList();

            return new EmployeeDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Salary = user.Salary,
                RoleId = user.RoleId,
                RoleName = role?.RoleName ?? "Unknown",
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                TotalSales = userOrders.Count,
                TotalRevenue = userOrders.Sum(o => o.TotalAmount)
            };
        }
    }
}