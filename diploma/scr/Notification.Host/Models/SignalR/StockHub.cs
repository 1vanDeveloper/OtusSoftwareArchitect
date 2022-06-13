using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Notification.Host.Helpers;
using StockMarket.Shared.Models;

namespace Notification.Host.Models.SignalR
{
    /// <summary>
    /// Настройка хаба сигналов
    /// </summary>
    public class StockHub : Hub
    {
        private readonly ILogger<StockHub> _logger;
        private static readonly object DataLocker = new();
        private static readonly IDictionary<DateTime, FinancialData> Data;
        private static readonly IDictionary<string, TimerManager> ClientConnections = new Dictionary<string, TimerManager>();

        static StockHub()
        {
            Data = StockHelper.GetHistoricalDataAsync().GetAwaiter().GetResult();
        }

        public StockHub(ILogger<StockHub> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        private async Task UpdateParametersAsync(int interval)
        {
            try
            {
                var connection = Context.ConnectionId;
                var client = Clients.Client(connection);
                await UpdatingConnectActionAsync(client, Data, true);
                if (!ClientConnections.ContainsKey(connection))
                {
                    ClientConnections.Add(connection, new TimerManager(async () => await UpdatingConnectActionAsync(client, Data), interval));
                } 
                else
                {
                    ClientConnections[connection].Stop();
                    ClientConnections[connection] = new TimerManager(async () => await UpdatingConnectActionAsync(client, Data), interval);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdateParameters");
            }
        }

        private static async Task Send(FinancialData[] array, IClientProxy client)
        {
            await client.SendAsync("TransferData", array);
        }

        private void StopTimer()
        {
            ClientConnections.TryGetValue(Context.ConnectionId, out var timerManager);
            timerManager?.Stop();
        }

        /// <inheritdoc />
        public override async Task OnConnectedAsync()
        {
            await UpdateParametersAsync(1000);
            await base.OnConnectedAsync();
        }

        /// <inheritdoc />
        public override Task OnDisconnectedAsync(Exception exception)
        {
            StopTimer();
            ClientConnections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        
        private async Task UpdatingConnectActionAsync(IClientProxy client, IDictionary<DateTime, FinancialData> data, bool all = false)
        {
            try
            {
                FinancialData[] sentData;
                lock (DataLocker)
                {
                    data.GenerateData(DateTime.Today);
                    sentData = all 
                        ? data.Values.ToArray() 
                        : new[] {data[DateTime.Today]};
                }
            
                await Send(sentData, client);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdatingConnectActionAsync");
            }
        }

    }
}