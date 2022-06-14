using System;
using System.IO;
using System.Reflection;
using System.Security.Authentication;
using System.Threading.Tasks;
using Autofac;
using Notification.Domain;
using Notification.Host.Attributes;
using Notification.Host.Middlewares;
using Notification.Host.Models.Events;
using Notification.Host.Services;
using Notification.Host.Settings;
using EventBus;
using EventBus.Abstractions;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.Host.Models.SignalR;
using Prometheus;
using Prometheus.SystemMetrics;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace Notification.Host
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration info
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings(Configuration);
            services.AddSingleton<IAppSettings>(appSettings);
            
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddControllers(cfg => { cfg.Filters.Add(new ValidateModelAttribute()); })
                .AddNewtonsoftJson();
            
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

            services.AddNotificationDomainServices(appSettings.DbConnectionString);
            services.AddSingleton<IInternalHttpService, InternalHttpService>();
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireClaim("scope", "standardService");
                });
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = appSettings.IdentityServerUrl;
                    options.RequireHttpsMetadata = false;
                    options.SupportedTokens = SupportedTokens.Both;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (string.IsNullOrEmpty(context.Token)) &&
                                (path.StartsWithSegments("/stock")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                ConfigurePolicy));
            
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            
            services.AddDataProtection();
            
            RegisterEventBus(services, appSettings);
            
            services.AddSystemMetrics();

            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(10);
            })
            .AddStackExchangeRedis(o =>
            {
                o.ConnectionFactory = async writer =>
                {
                    var connection = await ConnectionMultiplexer.ConnectAsync(appSettings.RedisConnectionString, writer);
                    connection.ConnectionFailed += (_, _) =>
                    {
                        Console.WriteLine($"Connection to Redis failed: {appSettings.RedisConnectionString}.");
                    };

                    if (!connection.IsConnected)
                    {
                        Console.WriteLine("Did not connect to Redis: {appSettings.RedisConnectionString}.");
                    }

                    return connection;
                };
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseExceptionHandlerMiddleware();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = "swagger";
            });

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseHttpMetrics();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization("ApiScope")
                    .RequireCors(ConfigurePolicy);
                endpoints.MapMetrics();
                endpoints.MapHealthChecks("/readiness", new HealthCheckOptions
                {
                    Predicate = _ => true
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });

                endpoints.MapHub<StockHub>("/stock",
                    options =>
                    {
                        options.ApplicationMaxBufferSize = 64;
                        options.TransportMaxBufferSize = 64;
                        options.LongPolling.PollTimeout = TimeSpan.FromMinutes(1);
                        options.Transports = HttpTransportType.LongPolling | HttpTransportType.WebSockets;
                    })
                    .RequireAuthorization("ApiScope")
                    .RequireCors(ConfigurePolicy);
                
                endpoints.MapHub<NotifyHub>("/notify",
                        options =>
                        {
                            options.ApplicationMaxBufferSize = 64;
                            options.TransportMaxBufferSize = 64;
                            options.LongPolling.PollTimeout = TimeSpan.FromMinutes(1);
                            options.Transports = HttpTransportType.LongPolling | HttpTransportType.WebSockets;
                        })
                    .RequireAuthorization("ApiScope")
                    .RequireCors(ConfigurePolicy);
            });

            ConfigureEventBus(app);
        }

        private static void RegisterEventBus(IServiceCollection services, IAppSettings settings)
        {
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = settings.EventBusConnection,
                    DispatchConsumersAsync = true,
                    AmqpUriSslProtocols = SslProtocols.None,
                    UserName = settings.EventBusUserName,
                    Password = settings.EventBusPassword
                };

                return new DefaultRabbitMQPersistentConnection(factory, logger);
            });

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = settings.QueueName;

                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(rabbitMqPersistentConnection, logger, iLifetimeScope,
                    eventBusSubscriptionsManager, subscriptionClientName);
            });
            
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            
            services.AddTransient<BillingEventHandler>();
        }
        
        private static void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<BillingEvent, BillingEventHandler>();
        }

        private static void ConfigurePolicy(CorsPolicyBuilder builder)
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true) // allow any origin
                .AllowCredentials();
        }
    }
}