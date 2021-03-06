using System.Linq;
using System.Threading.Tasks;
using Identity.Configuration;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Data
{
    /// <summary>
    /// Config params for <see cref="AppConfigurationDbContext"/>
    /// </summary>
    public static class ConfigurationDbContextSeed
    {
        /// <summary>
        /// Save params
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static async Task SeedAsync(AppConfigurationDbContext context, IConfiguration configuration)
        {
            context.IdentityResources.RemoveRange(await context.IdentityResources.ToArrayAsync());
            foreach (var resource in Config.GetResources())
            {
                await context.IdentityResources.AddAsync(resource.ToEntity());
            }
            await context.SaveChangesAsync();

            context.ApiResources.RemoveRange(await context.ApiResources.ToArrayAsync());
            foreach (var api in Config.GetApis())
            {
                await context.ApiResources.AddAsync(api.ToEntity());
            }
            await context.SaveChangesAsync();
            
            context.ApiScopes.RemoveRange(await context.ApiScopes.ToArrayAsync());
            foreach (var api in Config.GetScopes())
            {
                await context.ApiScopes.AddAsync(api.ToEntity());
            }
            await context.SaveChangesAsync();
            
            context.Clients.RemoveRange(await context.Clients.ToArrayAsync());
            foreach (var client in Config.GetClients())
            {
                await context.Clients.AddAsync(client.ToEntity());
            }
            await context.SaveChangesAsync();
        }
    }
}