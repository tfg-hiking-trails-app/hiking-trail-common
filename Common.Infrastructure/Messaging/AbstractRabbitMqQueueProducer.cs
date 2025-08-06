using Common.Domain.Interfaces.Messaging;
using RabbitMQ.Client;

namespace Common.Infrastructure.Messaging;

public abstract class AbstractRabbitMqQueueProducer : IRabbitMqQueueProducer
{
    private readonly IRabbitMqQueueProvider _channelProvider;
    
    public AbstractRabbitMqQueueProducer(IRabbitMqQueueProvider channelProvider)
    {
        _channelProvider = channelProvider;
    }
    
    public abstract string QueueName { get; }
    public abstract string ExchangeName { get; }

    public async Task BasicPublishAsync(byte[] body)
    {
        IChannel channel = await _channelProvider.GetChannelAsync(ExchangeName, QueueName);
        
        await channel.BasicPublishAsync(ExchangeName, QueueName, body);
    }
    
    protected string GetUsingEnvironmentVariable(string environmentVariableName)
    {
        string? result = Environment.GetEnvironmentVariable(environmentVariableName);
        
        if (string.IsNullOrEmpty(result))
            throw new Exception($"Environment variable {environmentVariableName} not found");
        
        return result;
    }
    
}