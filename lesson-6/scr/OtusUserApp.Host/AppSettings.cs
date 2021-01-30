using System.Text;
using Microsoft.Extensions.Configuration;
using OtusUserApp.Host;
using OtusUserApp.Host.Extensions;

namespace OtusUserApp.Host
{
    /// <inheritdoc />
    internal class AppSettings : IAppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            //UsersDbConnectionString = GetDbConnectionString(configuration); 

            UsersDbConnectionString = "Host=localhost;Port=7654;Database=otus-users;Username=postgres;Password=\"qweqwe123\";"; 
        }

        public string UsersDbConnectionString { get; }
        
        private static string GetDbConnectionString(IConfiguration configuration)
        {
            var dbHost = configuration.GetValueOrThrow<string>("USERS_PG_HOST");
            var dbDatabase = configuration.GetValueOrThrow<string>("USERS_PG_DATABASE");
            var dbUsername = configuration.GetValueOrThrow<string>("USERS_PG_USERNAME");
            var dbPassword = configuration.GetValueOrThrow<string>("USERS_PG_PASSWORD");
            var useSSL = configuration.GetValueOrThrow<bool>("USE_SSL_PG_CONNECTION");

            var dbPort = configuration.GetValueOrThrow<string>("USERS_PG_PORT");

            var connectionStringBuilder = new StringBuilder();

            connectionStringBuilder.Append($"Host={dbHost};");

            if (!string.IsNullOrWhiteSpace(dbPort))
            {
                connectionStringBuilder.Append($"Port={dbPort};");
            }

            connectionStringBuilder.Append($"Database={dbDatabase};");
            connectionStringBuilder.Append($"Username={dbUsername};");
            connectionStringBuilder.Append($"Password=\"{dbPassword}\";");
            if (useSSL)
            {
                connectionStringBuilder.Append("SslMode=Require;");
            }

            return connectionStringBuilder.ToString();
        }
    }
}
