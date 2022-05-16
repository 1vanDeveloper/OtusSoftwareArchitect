using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Order.Domain.Services;
using Order.Host.Models;
using Order.Host.Settings;
using Polly;

namespace Order.Host.Services
{
    /// <inheritdoc cref="IBillingService"/>
    public class BillingService: IBillingService
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger<BillingService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public BillingService(IAppSettings appSettings, ILogger<BillingService> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<(bool result, string message)> BuyAsync(long userId, decimal amount, string description, Guid operationId, CancellationToken cancellationToken)
        {
            var url = $"{_appSettings.BillingServiceUrl}/internal/buy/";
            var policy = Policy.Handle<TimeoutException>()
                .Or<SocketException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    if (ex.Message != null)
                        _logger.LogWarning(ex,
                            $"Could not connect to: {url} after {time.TotalSeconds:n1}s ({ex.Message})");
                });

            var context = new Context();
            (bool result, string message) result = (false, "Internal error");

            async Task Func(Context ct)
            {
                var buyParams = new BuyParamsDto
                {
                    Amount = amount,
                    Description = description,
                    OperationId = operationId,
                    UserId = userId
                };
                
                result = await InternalBuyAsync(url, buyParams, cancellationToken);
            }

            await policy.ExecuteAsync(Func, context);
            
            _logger.LogInformation($"Buy order by {url} {result.result}: {result.message}");

            return result;
        }

        private async Task<(bool result, string message)> InternalBuyAsync(string url, BuyParamsDto paramsDto, CancellationToken cancellationToken)
        {
            using var client = new HttpClient();
            var body = JsonConvert.SerializeObject(paramsDto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            
            _logger.LogInformation($"Request body: {body}");
            
            var response = await client.PostAsync(url,  content, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return (true, "Success");
            }
            
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            var error = JsonConvert.DeserializeObject<ErrorDto>(result);

            return (false, $"{error?.Code}: {error?.Message}");
        }
    }
}