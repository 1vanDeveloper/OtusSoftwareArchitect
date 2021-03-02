using ApiGateway.Extensions;
using Microsoft.Extensions.Configuration;

namespace ApiGateway.Settings
{
    /// <inheritdoc />
    internal class AppSettings : IAppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            IdentityServerUrl = configuration.GetValueOrThrow<string>("IDENTITY_SERVER_URL");
        }

        public string IdentityServerUrl { get; }
    }
}