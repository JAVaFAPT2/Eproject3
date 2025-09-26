using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Application.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Handlers
{
    /// <summary>
    /// Handler for retrieving users with filtering and pagination
    /// </summary>
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public GetUsersQueryHandler(IRepository<User> userRepository, IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var filterBuilder = Builders<User>.Filter;
            var filter = filterBuilder.Eq(u => u.IsDeleted, false);

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                var searchFilter = filterBuilder.Or(
                    filterBuilder.Regex(u => u.Username, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    filterBuilder.Regex(u => u.Email, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    filterBuilder.Regex(u => u.FirstName, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    filterBuilder.Regex(u => u.LastName, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
                );
                filter = filterBuilder.And(filter, searchFilter);
            }

            if (request.RoleId.HasValue)
            {
                filter = filterBuilder.And(filter, filterBuilder.Eq(u => u.RoleId, request.RoleId.Value.ToString()));
            }

            if (request.IsActive.HasValue)
            {
                filter = filterBuilder.And(filter, filterBuilder.Eq(u => u.IsActive, request.IsActive.Value));
            }

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            var options = new FindOptions<User>
            {
                Skip = skip,
                Limit = request.PageSize,
                Sort = Builders<User>.Sort.Ascending(u => u.CreatedAt)
            };

            // Execute query
            var allUsers = await _userRepository.GetAllAsync();
            var users = allUsers.Where(u => !u.IsDeleted).ToList();

            // Apply MongoDB filters manually since we can't use EF-style queries
            var filteredUsers = users.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                filteredUsers = filteredUsers.Where(u =>
                    u.Username.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    u.FirstName.ToLower().Contains(searchTerm) ||
                    u.LastName.ToLower().Contains(searchTerm));
            }

            if (request.RoleId.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.RoleId == request.RoleId.Value.ToString());
            }

            if (request.IsActive.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.IsActive == request.IsActive.Value);
            }

            // Apply pagination
            var paginatedUsers = filteredUsers.Skip(skip).Take(request.PageSize);

            // Map to DTOs
            return paginatedUsers.Select(MapToDto).ToList();
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = user.RoleId,
                RoleName = user.Role?.RoleName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}