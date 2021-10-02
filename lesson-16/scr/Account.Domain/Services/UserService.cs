using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Account.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Account.Domain.Services
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
        public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            if (await _dbContext.Users.AnyAsync(u => u.UserName == user.UserName, cancellationToken))
            {
                throw new Exception("This user name has already existed");
            }
            
            var createdUser = (await _dbContext.Users.AddAsync(user, cancellationToken)).Entity;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return createdUser;
        }

        /// <inheritdoc />
        public async Task<User> GetUserAsync(string userName, CancellationToken cancellationToken)
        {
            var dbUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == userName, cancellationToken);
            if (dbUser == null)
            {
                throw new KeyNotFoundException();
            }

            return dbUser;
        }

        /// <inheritdoc />
        public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            var dbUser = await _dbContext.Users
                .SingleOrDefaultAsync(g => g.Id == user.Id, cancellationToken);
            if (dbUser == null)
            {
                throw new KeyNotFoundException();
            }

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;
            dbUser.Phone = user.Phone;

            _dbContext.Update(dbUser);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task RemoveUserAsync(string userName, CancellationToken cancellationToken)
        {
            var dbUser = await _dbContext.Users
                .SingleOrDefaultAsync(g => g.UserName == userName, cancellationToken);
            if (dbUser == null)
            {
                throw new KeyNotFoundException();
            }

            _dbContext.Remove(dbUser);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}