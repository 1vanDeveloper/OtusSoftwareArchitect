using System.Text;
using Notification.Host.Extensions;
using Microsoft.Extensions.Configuration;

namespace Notification.Host.Settings
{
    /// <inheritdoc />
    internal class AppSettings : IAppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            DbConnectionString = GetDbConnectionString(configuration);
            RedisConnectionString = GetRedisConnectionString(configuration);
            IsMigrationService = configuration.GetValue<bool>("MIGRATION_MODE");
            IdentityServerUrl = configuration.GetValue<string>("IDENTITY_SERVER_URL");
            AccountServiceUrl = configuration.GetValue<string>("ACCOUNT_SERVICE_URL");
            
            EventBusConnection = configuration.GetValue<string>("EVENT_BUS_CONNECTION");
            EventBusUserName = configuration.GetValue<string>("EVENT_BUS_USERNAME");
            EventBusPassword = configuration.GetValue<string>("EVENT_BUS_PASSWORD");
            QueueName = configuration.GetValue<string>("QUEUE_NAME");
            //UsersDbConnectionString = "Host=localhost;Port=7654;Database=otus-users;Username=postgres;Password=\"qweqwe123\";"; 
        }

        public string DbConnectionString { get; }
        
        public string RedisConnectionString { get; }

        public bool IsMigrationService { get; }
        
        public string IdentityServerUrl { get; }
        
        public string AccountServiceUrl { get; }

        public string EventBusConnection { get; }
        
        public string EventBusUserName { get; }
        
        public string EventBusPassword { get; }

        public string QueueName { get; }

        private static string GetRedisConnectionString(IConfiguration configuration)
        {
            var dbHost = configuration.GetValueOrThrowBySuffix<string>("REDIS_SERVICE_REPLICAS_SERVICE_HOST");
            var dbPort = configuration.GetValueOrThrowBySuffix<string>("REDIS_SERVICE_REPLICAS_SERVICE_PORT");
            
            var connectionStringBuilder = new StringBuilder();
            connectionStringBuilder.Append($"{dbHost}");

            if (!string.IsNullOrWhiteSpace(dbPort))
            {
                connectionStringBuilder.Append($":{dbPort}");
            }
            
            return connectionStringBuilder.ToString();
        }
        
        private static string GetDbConnectionString(IConfiguration configuration)
        {
            var dbHost = configuration.GetValueOrThrowBySuffix<string>("POSTGRESQL_SERVICE_HOST");
            var dbPort = configuration.GetValueOrThrowBySuffix<string>("POSTGRESQL_SERVICE_PORT");
            var dbDatabase = configuration.GetValueOrThrow<string>("NOTIFICATION_PG_DATABASE");
            var dbUsername = configuration.GetValueOrThrow<string>("NOTIFICATION_PG_USERNAME");
            var dbPassword = configuration.GetValueOrThrow<string>("NOTIFICATION_PG_PASSWORD");
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