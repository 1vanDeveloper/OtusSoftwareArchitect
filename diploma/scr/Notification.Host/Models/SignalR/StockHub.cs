using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Notification.Host.Helpers;
using StockMarket.Shared.Models;

namespace Notification.Host.Models.SignalR
{
    /// <summary>
    /// Настройка хаба сигналов
    /// </summary>
    public class StockHub : Hub
    {
        private static readonly object DataLocker = new();
        private static readonly IDictionary<DateTime, FinancialData> Data;
        private static readonly IDictionary<string, TimerManager> ClientConnections = new Dictionary<string, TimerManager>();

        static StockHub()
        {
            Data = StockHelper.GetHistoricalDataAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateParameters(int interval)
        {
            var connection = Context.ConnectionId;
            if (!ClientConnections.ContainsKey(connection))
            {
                ClientConnections.Add(connection, new TimerManager(async () => await UpdatingConnectActionAsync(connection), interval));
            } 
            else
            {
                ClientConnections[connection].Stop();
                ClientConnections[connection] = new TimerManager(async () => await UpdatingConnectActionAsync(connection), interval);
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
        public override Task OnConnectedAsync()
        {
            UpdateParameters(1000);
            return base.OnConnectedAsync();
        }

        /// <inheritdoc />
        public override Task OnDisconnectedAsync(Exception exception)
        {
            StopTimer();
            ClientConnections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        
        private async Task UpdatingConnectActionAsync(string connection)
        {
            var client = Clients.Client(connection);
            FinancialData[] sentData;
            lock (DataLocker)
            {
                Data.GenerateData(DateTime.Today);
                sentData = Data.Values.ToArray();
            }
            
            await Send(sentData, client);
        }

    }
}