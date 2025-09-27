using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    /// <summary>
    /// MongoDB implementation of User repository
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id && !u.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email && !u.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username && !u.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _users.Find(u => !u.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(Domain.Enums.UserRole role)
        {
            return await _users.Find(u => u.Role == role && !u.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task DeleteAsync(string id)
        {
            var update = Builders<User>.Update
                .Set(u => u.IsDeleted, true)
                .Set(u => u.DeletedAt, DateTime.UtcNow);
            
            await _users.UpdateOneAsync(u => u.Id == id, update);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var count = await _users.CountDocumentsAsync(u => u.Id == id && !u.IsDeleted);
            return count > 0;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var count = await _users.CountDocumentsAsync(u => u.Email == email && !u.IsDeleted);
            return count > 0;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            var count = await _users.CountDocumentsAsync(u => u.Username == username && !u.IsDeleted);
            return count > 0;
        }
    }
}