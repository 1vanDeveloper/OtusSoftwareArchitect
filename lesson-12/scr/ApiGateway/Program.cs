using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Settings;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ApiGateway
{
    public static class Program
    {
        private static readonly string Namespace = typeof(Startup).Namespace;
        private static readonly string AppName = Namespace.Contains('.') 
            ? Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1) 
            : Namespace;
        
        public static async Task<int> Main(string[] args)
        {
           try
           {
               var configuration = GetConfiguration();
               Log.Logger = CreateSerilogLogger(configuration, AppName);
                
               Log.Information("Configuring web host ({ApplicationContext})...", AppName);
               var host = BuildWebHost(configuration, args);
                
               LogEnvironmentAndSettings(host.Services);

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
                .ConfigureAppConfiguration(x =>
                {
                    x.AddConfiguration(configuration);
                })
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .Build();
        
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
        
        private static ILogger CreateSerilogLogger(IConfiguration configuration, string appName)
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
            var configuration = serviceProvider.GetService<IConfiguration>();
            var appSettings = serviceProvider.GetService<IAppSettings>();
            if (appSettings == null)
            {
                return;
            }
            
            var allConfiguration = configuration.AsEnumerable().Concat(new[]
                {
                    new KeyValuePair<string, string>(nameof(appSettings.IdentityServerUrl),
                        appSettings.IdentityServerUrl),
                })
                .Select(pair => $"'{pair.Key}={pair.Value}'");

            var resString = string.Join("\n", allConfiguration);

            const string logErrorInfo = "Application IConfiguration contains:\n{0}";
            
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            Log.Information(string.Format(logErrorInfo, resString));
        }
    }
}