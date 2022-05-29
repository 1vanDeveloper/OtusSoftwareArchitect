using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Notification.Host.Helpers;

namespace Notification.Host.Models.SignalR
{
    /// <summary>
    /// Настройка хаба сигналов
    /// </summary>
    public class StockHub : Hub
    {
        private static readonly IDictionary<string, TimerManager> ClientConnections = new Dictionary<string, TimerManager>();
        private readonly StockHelper _helper = new ();

        public async void UpdateParametersAsync(int interval, int volume, bool live = false, bool updateAll = true)
        {
            var dataArray = JsonConvert.DeserializeObject<List<FinancialData>>(_helper.jsonData)?.ToArray();
            var newDataArray = _helper.GenerateData(dataArray, volume);
            var connection = Context.ConnectionId;
            var clients = Clients;

            if (live)
            {
                if (!ClientConnections.ContainsKey(connection))
                {
                    ClientConnections.Add(connection, new TimerManager(async () => await NewConnectActionAsync(newDataArray, updateAll), interval));
                } 
                else
                {
                    ClientConnections[connection].Stop();
                    ClientConnections[connection] = new TimerManager(async () => await UpdatingConnectActionAsync(newDataArray, updateAll), interval);
                }
            }
            else
            {
                var client = clients.Client(connection);
                await Send(newDataArray, client);
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
        public override Task OnDisconnectedAsync(Exception exception)
        {
            StopTimer();
            ClientConnections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        
        private async Task NewConnectActionAsync(FinancialData[] newDataArray, bool updateAll = true)
        {
            var connection = Context.ConnectionId;
            var clients = Clients;
            
            var client = clients.Client(connection);
            if (updateAll)
            {
                StockHelper.UpdateAllPrices(newDataArray);
            }
            else
            {
                StockHelper.UpdateRandomPrices(newDataArray);
            }

            await Send(newDataArray, client);
        }
        
        private async Task UpdatingConnectActionAsync(FinancialData[] newDataArray, bool updateAll = true)
        {
            var connection = Context.ConnectionId;
            var clients = Clients;
            
            var client = clients.Client(connection);
            if (updateAll)
            {
                StockHelper.UpdateAllPrices(newDataArray);
            }
            else
            {
                StockHelper.UpdateRandomPrices(newDataArray);
            }

            await Send(newDataArray, client);
        }

    }
}