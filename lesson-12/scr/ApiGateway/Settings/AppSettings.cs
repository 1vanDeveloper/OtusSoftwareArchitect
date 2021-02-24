using System.Text;
using Identity.Extensions;
using Microsoft.Extensions.Configuration;

namespace Identity.Settings
{
    /// <inheritdoc />
    internal class AppSettings : IAppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            ConnectionString = GetDbConnectionString(configuration); 
            IsMigrationService = configuration.GetValueOrThrow<bool>("MIGRATION_MODE");
            IsInKubernetes = configuration.IsInKubernetes();
            //ConnectionString = "Host=localhost;Port=7654;Database=otus-identity;Username=postgres;Password=\"qweqwe123\";"; 
        }

        public string ConnectionString { get; }

        public bool IsMigrationService { get; }
        
        public bool IsInKubernetes { get; }

        private static string GetDbConnectionString(IConfiguration configuration)
        {
            var dbHost = configuration.GetValueOrThrowBySuffix<string>("POSTGRESQL_SERVICE_HOST");
            var dbPort = configuration.GetValueOrThrowBySuffix<string>("POSTGRESQL_SERVICE_PORT");
            var dbDatabase = configuration.GetValueOrThrow<string>("USERS_PG_DATABASE");
            var dbUsername = configuration.GetValueOrThrow<string>("USERS_PG_USERNAME");
            var dbPassword = configuration.GetValueOrThrow<string>("USERS_PG_PASSWORD");
            var useSsl = configuration.GetValueOrThrow<bool>("USE_SSL_PG_CONNECTION");

            var connectionStringBuilder = new StringBuilder();
            connectionStringBuilder.Append($"Host={dbHost};");

            if (!string.IsNullOrWhiteSpace(dbPort))
            {
                connectionStringBuilder.Append($"Port={dbPort};");
            }

            connectionStringBuilder.Append($"Database={dbDatabase};");
            connectionStringBuilder.Append($"Username={dbUsername};");
            connectionStringBuilder.Append($"Password=\"{dbPassword}\";");
            if (useSsl)
            {
                connectionStringBuilder.Append("SslMode=Require;");
            }

            return connectionStringBuilder.ToString();
        }
    }
}