using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data;
using Identity.Extensions;
using Identity.Settings;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Identity
{
    /// <summary>
    /// Main class
    /// </summary>
    public class Program
    {
        private static readonly string Namespace = typeof(Startup).Namespace;
        private static readonly string AppName = Namespace.Contains('.') 
            ? Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1) 
            : Namespace;
        
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<int> Main(string[] args)
        {
            try
            {
                var configuration = GetConfiguration();
                Log.Logger = CreateSerilogLogger(configuration, AppName);
                
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = BuildWebHost(configuration, args);
                
                LogEnvironmentAndSettings(host.Services);

                var settings = host.Services.GetService<IAppSettings>();
                if (settings?.IsMigrationService ?? false)
                {
                    Log.Information("Start applying migrations ({ApplicationContext})...", AppName);
                    await host.MigrateDbContextAsync<AppPersistedGrantDbContext>((_, __) => Task.CompletedTask);
                    await host.MigrateDbContextAsync<ApplicationDbContext>((context, services) =>
                    {
                        var env = services.GetService<IWebHostEnvironment>();
                        var logger = services.GetService<ILogger<ApplicationDbContextSeed>>();
                        return new ApplicationDbContextSeed()
                            .SeedAsync(context, env, logger);
                    });
                    await host.MigrateDbContextAsync<AppConfigurationDbContext>((context, services) => ConfigurationDbContextSeed.SeedAsync(context, configuration));

                    Log.Information("Finish applying migrations ({ApplicationContext})...", AppName);
                    if (settings.IsInKubernetes)
                    {
                        return 0;
                    }
                }

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                await host.RunAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .Build();
        
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
        
        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration, string appName)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        
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
                    new KeyValuePair<string, string>(nameof(appSettings.ConnectionString),
                        appSettings.ConnectionString),
                })
                .Select(pair => $"'{pair.Key}={pair.Value}'");

            var resString = string.Join("\n", allConfiguration);

            const string logErrorInfo = "Application IConfiguration contains:\n{0}";
            
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.LogInformation(string.Format(logErrorInfo, resString));
        }
    }
}