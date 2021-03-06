using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Identity.Extensions
{
    /// <summary>
    /// IWebHostExtensions helper
    /// </summary>
    public static class WebHostExtensions
    {
        /// <summary>
        /// Check Kubernetes runtime
        /// </summary>
        /// <param name="webHost"></param>
        /// <returns></returns>
        public static bool IsInKubernetes(this IWebHost webHost)
        {
            var cfg = webHost.Services.GetService<IConfiguration>();
            return cfg.IsInKubernetes();
        }
        
        /// <summary>
        /// Check Kubernetes runtime
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static bool IsInKubernetes(this IConfiguration cfg)
        {
            var orchestratorType = cfg.GetValueOrThrow<string>("ORCHESTRATOR_TYPE");
            return orchestratorType?.ToUpper() == "K8S";
        }

        /// <summary>
        /// Run Migration
        /// </summary>
        /// <param name="webHost"></param>
        /// <param name="seeder"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static async Task<IWebHost> MigrateDbContextAsync<TContext>(this IWebHost webHost,
            Func<TContext, IServiceProvider, Task> seeder) where TContext : DbContext
        {
            var underK8S = webHost.IsInKubernetes();

            using var scope = webHost.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}",
                    typeof(TContext).Name);

                if (underK8S)
                {
                    await InvokeSeederAsync(seeder, context, services);
                }
                else
                {
                    var retries = 10;
                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetryAsync(
                            retryCount: retries,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                            onRetry: (exception, _, i, _) =>
                            {
                                logger.LogWarning(exception,
                                    "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}",
                                    nameof(TContext), exception.GetType().Name, exception.Message, i, retries);
                            });

                    //if the sql server container is not created on run docker compose this
                    //migration can't fail for network related exception. The retry options for DbContext only 
                    //apply to transient exceptions
                    // Note that this is NOT applied when running some orchestrators (let the orchestrator to recreate the failing service)
                    await retry.ExecuteAsync(() => InvokeSeederAsync(seeder, context, services));
                }

                logger.LogInformation("Migrated database associated with context {DbContextName}",
                    typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "An error occurred while migrating the database used on context {DbContextName}",
                    typeof(TContext).Name);
                if (underK8S)
                {
                    throw; // Rethrow under k8s because we rely on k8s to re-run the pod
                }
            }

            return webHost;
        }

        private static async Task InvokeSeederAsync<TContext>(Func<TContext, IServiceProvider, Task> seeder,
            TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            await context.Database.MigrateAsync();
            await seeder(context, services);
        }
    }
}