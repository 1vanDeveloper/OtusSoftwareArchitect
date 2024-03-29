using System;
using RabbitMQ.Client;

namespace EventBus
{
    public interface IRabbitMQPersistentConnection: IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}