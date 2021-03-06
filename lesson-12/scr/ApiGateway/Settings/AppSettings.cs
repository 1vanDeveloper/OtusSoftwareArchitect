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
            UsePodServiceAccount = configuration.GetValueOrThrow<bool>("USE_POD_SERVICE_ACCOUNT");
            IsInKubernetes = IsInKube(configuration);
        }

        public string IdentityServerUrl { get; }
        
        public bool IsInKubernetes { get; }
        
        public bool UsePodServiceAccount { get; }

        /// <summary>
        /// Check Kubernetes runtime
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        private bool IsInKube(IConfiguration cfg)
        {
            var orchestratorType = cfg.GetValueOrThrow<string>("ORCHESTRATOR_TYPE");
            return orchestratorType?.ToUpper() == "K8S";
        }
    }
}