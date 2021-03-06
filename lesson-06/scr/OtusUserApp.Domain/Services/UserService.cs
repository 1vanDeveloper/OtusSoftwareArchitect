using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OtusUserApp.Domain.Models;

namespace OtusUserApp.Domain.Services
{
    /// <inheritdoc cref="IUserService"/>
    public class UserService: IUserService
    {
        private readonly AppDbContext _dbContext;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <inheritdoc />
        public async Task<User> CreateUserAsync(User user)
        {
            if (await _dbContext.Users.AnyAsync(u => u.UserName == user.UserName))
            {
                throw new Exception("This user name has already existed");
            }
            
            var createdUser = (await _dbContext.Users.AddAsync(user)).Entity;
            await _dbContext.SaveChangesAsync();

            return createdUser;
        }

        /// <inheritdoc />
        public async Task<User> GetUserAsync(long userId)
        {
            var dbUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (dbUser == null)
            {
                throw new KeyNotFoundException();
            }

            return dbUser;
        }

        /// <inheritdoc />
        public async Task UpdateUserAsync(User user)
        {
            var dbUser = await _dbContext.Users
                .SingleOrDefaultAsync(g => g.Id == user.Id);
            if (dbUser == null)
            {
                throw new KeyNotFoundException();
            }

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;
            dbUser.Phone = user.Phone;

            _dbContext.Update(dbUser);

            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task RemoveUserAsync(long userId)
        {
            var dbUser = await _dbContext.Users
                .SingleOrDefaultAsync(g => g.Id == userId);
            if (dbUser == null)
            {
                throw new KeyNotFoundException();
            }

            _dbContext.Remove(dbUser);
            await _dbContext.SaveChangesAsync();
        }
    }
}