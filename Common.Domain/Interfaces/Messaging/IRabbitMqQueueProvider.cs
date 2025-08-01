using RabbitMQ.Client;

namespace Common.Domain.Interfaces.Messaging;

public interface IRabbitMqQueueProvider
{
    Task<IChannel> GetChannelAsync(string exchangeName, string queueName);
}