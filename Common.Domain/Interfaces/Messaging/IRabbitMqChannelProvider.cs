using RabbitMQ.Client;

namespace Common.Domain.Interfaces.Messaging;

public interface IRabbitMqChannelProvider : IDisposable
{
    Task<IChannel> GetChannelAsync();
}