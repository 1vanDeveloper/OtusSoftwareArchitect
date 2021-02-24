using System.Linq;
using System.Threading.Tasks;
using Identity.Configuration;
using IdentityServer4.EntityFramework.Mappers;
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

            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.GetResources())
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var api in Config.GetApis())
                {
                    await context.ApiResources.AddAsync(api.ToEntity());
                }

                await context.SaveChangesAsync();
            }
            
            if (!context.ApiScopes.Any())
            {
                foreach (var api in Config.GetScopes())
                {
                    await context.ApiScopes.AddAsync(api.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }
    }
}