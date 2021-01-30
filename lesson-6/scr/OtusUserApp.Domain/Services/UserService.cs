using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OtusUserApp.Domain.Models;

namespace OtusUserApp.Domain.Services
{
    /// <inheritdoc cref="IUserService"/>
    public class UserService: IUserService
    {
        private readonly AppDbContext _dbContext;
        
        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<User> CreateUserAsync(User user)
        {
            var createdUser = (await _dbContext.Users.AddAsync(user)).Entity;
            await _dbContext.SaveChangesAsync();

            return createdUser;
        }

        public Task<User> GetUserAsync(long userId)
        {
            return _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            var dbUser = await _dbContext.Users
                .SingleOrDefaultAsync(g => g.Id == user.Id) ?? throw new KeyNotFoundException($"User (with id {user.Id}) is not found");

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;
            dbUser.Phone = user.Phone;

            _dbContext.Update(dbUser);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(long userId)
        {
            var dbUser = await _dbContext.Users
                .SingleOrDefaultAsync(g => g.Id == userId) ?? throw new KeyNotFoundException($"User (with id {userId}) is not found");

            _dbContext.Remove(dbUser);
            await _dbContext.SaveChangesAsync();
        }
    }
}