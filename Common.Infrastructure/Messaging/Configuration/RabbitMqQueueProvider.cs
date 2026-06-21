using Common.Domain.Interfaces.Messaging;
using RabbitMQ.Client;

namespace Common.Infrastructure.Messaging.Configuration;

public class RabbitMqQueueProvider : IRabbitMqQueueProvider
{
    private readonly IRabbitMqChannelProvider _channelProvider;

    public RabbitMqQueueProvider(IRabbitMqChannelProvider channelProvider)
    {
        _channelProvider = channelProvider;
    }
    
    public async Task<IChannel> GetChannelAsync(string exchangeName, string queueName)
    {
        IChannel channel = await _channelProvider.GetChannelAsync();

        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
        await channel.QueueDeclareAsync(queueName, true, false, false);
        await channel.QueueBindAsync(queueName, exchangeName, queueName);

        return channel;
    }
}