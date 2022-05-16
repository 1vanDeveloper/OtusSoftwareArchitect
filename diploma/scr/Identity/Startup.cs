using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Identity.Attributes;
using Identity.Data;
using Identity.Extensions;
using Identity.Models;
using Identity.Services;
using Identity.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace Identity
{
    /// <summary>
    /// Startup class
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
        /// Configuration settings
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings(Configuration);
            services.AddSingleton<IAppSettings>(appSettings);
            
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options => DbContextOptions(options, appSettings));
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());
            
            // Adds IdentityServer
            services.AddIdentityServer(x =>
                {
                    x.IssuerUri = appSettings.IdentityServerUrl;
                    x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore<AppConfigurationDbContext>(options => options.ConfigureDbContext = builder => DbContextOptions(builder, appSettings))
                .AddOperationalStore<AppPersistedGrantDbContext>(options => options.ConfigureDbContext = builder => DbContextOptions(builder, appSettings))
                .AddDeveloperSigningCredential()
                
                .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>()
                .Services.AddTransient<ILoginService<ApplicationUser>, EfLoginService>();
            
            services.AddControllers(cfg => { cfg.Filters.Add(new ValidateModelAttribute()); })
                .AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            
            // add CORS policy for non-IdentityServer endpoints
            services.AddCors(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            
            var container = new ContainerBuilder();
            container.Populate(services);
            
            return new AutofacServiceProvider(container.Build());
        }

        private static void DbContextOptions(DbContextOptionsBuilder options, IAppSettings appSettings)
        {
            options.UseNpgsql(appSettings.ConnectionString,
                builder =>
                {
                    builder.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                    builder.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
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
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity v1");
                c.RoutePrefix = "swagger";
            });

            // app.UseHttpsRedirection();
            app.UseExceptionHandlerMiddleware();

            // Make work identity server redirections in Edge and lastest versions of browers. WARN: Not valid in a production environment.
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "script-src 'unsafe-inline'");
                await next();
            });

            // app.UseForwardedHeaders();
            // Adds IdentityServer
            app.UseIdentityServer();

            // Fix a problem with chrome. Chrome enabled a new feature "Cookies without SameSite must be secure", 
            // the coockies shold be expided from https, but in eShop, the internal comunicacion in aks and docker compose is http.
            // To avoid this problem, the policy of cookies shold be in Lax mode.
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax,
                HttpOnly = HttpOnlyPolicy.None,
                Secure = CookieSecurePolicy.None
            });
            app.UseRouting();
            app.UseHttpMetrics();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
                endpoints.MapHealthChecks("/readiness", new HealthCheckOptions
                {
                    Predicate = _ => true
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }
    }
}