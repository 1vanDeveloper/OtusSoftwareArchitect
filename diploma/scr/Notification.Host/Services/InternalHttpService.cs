using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Notification.Host.Models;
using Notification.Host.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client.Exceptions;

namespace Notification.Host.Services
{
    /// <inheritdoc cref="IInternalHttpService"/>
    public class InternalHttpService: IInternalHttpService
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger<InternalHttpService> _logger;

        /// <summary>
        /// Costructor
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="logger"></param>
        public InternalHttpService(IAppSettings appSettings, ILogger<InternalHttpService> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<UserDto> GetUserAsync(string userName, CancellationToken cancellationToken)
        {
            var url = $"{_appSettings.AccountServiceUrl}/internal/account/{userName}";
            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    if (ex.Message != null)
                        _logger.LogWarning(ex,
                            $"Could not connect to: {url} after {time.TotalSeconds:n1}s ({ex.Message})");
                });

            var context = new Context();
            UserDto result = null;

            async Task Func(Context ct)
            {
                result = await InternalGetUserAsync(url, cancellationToken);
            }

            await policy.ExecuteAsync(Func, context);

            return result;
        }

        private async Task<UserDto> InternalGetUserAsync(string url, CancellationToken cancellationToken)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url, cancellationToken);
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation($"Get user by {url} {response.StatusCode}: {result}");

            return JsonConvert.DeserializeObject<UserDto>(result);
        }
    }
}