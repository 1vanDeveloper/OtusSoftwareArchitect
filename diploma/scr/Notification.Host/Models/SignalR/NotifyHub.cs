using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Notification.Host.Extensions;
using Notification.Host.Services;
using BillingEvent = Notification.Host.Models.Events.BillingEvent;

namespace Notification.Host.Models.SignalR
{
    /// <summary>
    /// Настройка хаба оповещений
    /// </summary>
    public class NotifyHub : Hub
    {
        private readonly ILogger<NotifyHub> _logger;
        private readonly IInternalHttpService _internalHttpService;
        private static readonly object DataLocker = new();
        private static readonly IDictionary<long, IList<string>> ClientConnections = new Dictionary<long, IList<string>>();

        /// <summary>
        /// Получение списка подключений пользователя
        /// </summary>
        public static IList<string> GetConnections(long userId)
        {
            lock (DataLocker)
            {
                return ClientConnections.TryGetValue(userId, out var connections)
                    ? connections.ToImmutableList()
                    : ImmutableList<string>.Empty;
            }
        }

        public NotifyHub(ILogger<NotifyHub> logger, IInternalHttpService internalHttpService)
        {
            _logger = logger;
            _internalHttpService = internalHttpService;
        }

        /// <summary>
        /// 
        /// </summary>
        private async Task UpdateParametersAsync()
        {
            try
            {
                var connection = Context.ConnectionId;
                var userId = await GetUserIdAsync();
                if (userId == 0)
                {
                    return;
                }

                lock (DataLocker)
                {
                    if (!ClientConnections.ContainsKey(userId))
                    {
                        ClientConnections.Add(userId, new List<string> { connection });
                    } 
                    else
                    {
                        ClientConnections[userId].Add(connection);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdateParameters");
            }
        }

        /// <summary>
        /// Отправка сообщения пользователю
        /// </summary>
        public static async Task Notify(BillingEvent @event, IClientProxy client)
        {
            await client.SendAsync("Notify", @event);
        }

        /// <inheritdoc />
        public override async Task OnConnectedAsync()
        {
            await UpdateParametersAsync();
            await base.OnConnectedAsync();
        }

        /// <inheritdoc />
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connection = Context.ConnectionId;
            lock (DataLocker)
            {
                var connections = ClientConnections.Values.FirstOrDefault(v => v.Contains(connection));
                connections?.Remove(Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }

        private async Task<long> GetUserIdAsync()
        {
            var currentUserName = Context.User?.Claims.GetUserName();
            if (string.IsNullOrWhiteSpace(currentUserName))
            {
                return 0;
            }

            var user = await _internalHttpService.GetUserAsync(currentUserName, CancellationToken.None);
            return user?.Id ?? 0;
        }
    }
}