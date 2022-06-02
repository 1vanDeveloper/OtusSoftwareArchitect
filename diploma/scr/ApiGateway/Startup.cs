using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Extensions;
using ApiGateway.Settings;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Prometheus;

namespace ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings(Configuration);

            // NUGET - Microsoft.AspNetCore.Authentication.JwtBearer
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = appSettings.IdentityServerUrl;
                    options.RequireHttpsMetadata = false;
                    options.SupportedTokens = SupportedTokens.Both;
                });

            services.AddCors();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

            if (appSettings.IsInKubernetes)
            {
                services.AddOcelot()
                    .AddKubernetesFixed(appSettings.UsePodServiceAccount);
            }
            else
            {
                services.AddOcelot();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true) // allow any origin
                .AllowCredentials());

            app.UseHttpMetrics();

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

            app.UseWebSockets();

            var configuration = new OcelotPipelineConfiguration
            {
                PreQueryStringBuilderMiddleware = PreErrorResponderMiddleware
            };

            await app.UseOcelot(configuration);
        }

        
        private static async Task PreErrorResponderMiddleware(HttpContext context, Func<Task> next)
        {
            if (!context.Response.Headers.ContainsKey(HeaderNames.AccessControlAllowOrigin))
            {
                context.Response.Headers.Add(
                    new KeyValuePair<string, StringValues>(HeaderNames.AccessControlAllowOrigin, new[] {"*"}));
            }

            if (!context.Response.Headers.ContainsKey(HeaderNames.AccessControlAllowHeaders))
            {
                context.Response.Headers.Add(
                    new KeyValuePair<string, StringValues>(HeaderNames.AccessControlAllowHeaders, new[] {"*"}));
            }

            if (!context.Response.Headers.ContainsKey(HeaderNames.AccessControlRequestMethod))
            {
                context.Response.Headers.Add(
                    new KeyValuePair<string, StringValues>(HeaderNames.AccessControlRequestMethod, new[] {"*"}));
            }

            await next.Invoke();
        }
    }
}