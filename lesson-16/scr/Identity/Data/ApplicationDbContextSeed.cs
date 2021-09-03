using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Data
{
    /// <summary>
    /// Config params for <see cref="ApplicationDbContext"/>
    /// </summary>
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        /// <summary>
        /// Save params
        /// </summary>
        /// <param name="context"></param>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        /// <param name="retry"></param>
        /// <returns></returns>
        public async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, int? retry = 0)
        {
            var retryForAvailability = retry ?? 0;

            try
            {
                if (!context.Users.Any())
                {
                    await context.Users.AddRangeAsync(GetDefaultUser());
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));

                    await SeedAsync(context, env, logger, retryForAvailability);
                }
            }
        }
        
        private IEnumerable<ApplicationUser> GetDefaultUser()
        {
            var user =
                new ApplicationUser()
                {
                    Email = "demouser@microsoft.com",
                    Id = Guid.NewGuid().ToString(),
                    PhoneNumber = "1234567890",
                    UserName = "demouser@microsoft.com",
                    NormalizedEmail = "DEMOUSER@MICROSOFT.COM",
                    NormalizedUserName = "DEMOUSER@MICROSOFT.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");
            return new List<ApplicationUser>
            {
                user
            };
        }

    }
}