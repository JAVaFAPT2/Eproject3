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
            var users = await _userRepository.GetAllQueryable()
                .Where(u => !u.IsDeleted && u.IsActive)
                .ToListAsync(cancellationToken);

            var roles = await _roleRepository.GetAllQueryable()
                .Where(r => !r.IsDeleted)
                .ToListAsync(cancellationToken);

            var orders = await _orderRepository.GetAllQueryable()
                .Where(o => !o.IsDeleted && o.Status == "COMPLETED")
                .ToListAsync(cancellationToken);

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

            return paginatedUsers.Select(MapToDto).ToList();
        }

        private EmployeeDto MapToDto(User user)
        {
            var role = _roleRepository.GetByIdAsync(user.RoleId).Result;
            var userOrders = _orderRepository.GetAllQueryable()
                .Where(o => o.SalesPersonId == user.Id && !o.IsDeleted)
                .Result ?? new List<SalesOrder>();

            return new EmployeeDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Role?.RoleId, // Using RoleId as phone for now
                Salary = 1500, // Default salary - would need proper field
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