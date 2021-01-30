using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OtusUserApp.Domain;
using host = Microsoft.Extensions.Hosting.Host;

namespace OtusUserApp.Host
{
    /// <summary>
    /// The main class of program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The start function
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            LogEnvironmentAndSettings(host.Services);

            using (var serviceScope = host.Services.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var serviceProvider = serviceScope?.ServiceProvider;
                var context = serviceProvider?.GetRequiredService<AppDbContext>();
                context?.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
                context?.Database.Migrate();
                context?.Database.SetCommandTimeout(TimeSpan.FromSeconds(30));
            }

            host.Run();
        }

        /// <summary>
        /// Создание хоста приложения
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        private static void LogEnvironmentAndSettings(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var configuration = serviceProvider.GetService<IConfiguration>();
            var appSettings = serviceProvider.GetService<IAppSettings>();
            if (appSettings == null)
            {
                return;
            }
            
            var allConfiguration = configuration.AsEnumerable().Concat(new[]
                {
                    new KeyValuePair<string, string>(nameof(appSettings.UsersDbConnectionString),
                        appSettings.UsersDbConnectionString),
                })
                .Select(pair => $"'{pair.Key}={pair.Value}'");

            var resString = string.Join("\n", allConfiguration);

            const string logErrorInfo = "Application IConfiguration contains:\n{0}";
            
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.LogInformation(string.Format(logErrorInfo, resString));
        }
    }
}
