namespace Common.Domain.Interfaces.Messaging;

public interface IRabbitMqQueueProducer
{
    Task BasicPublishAsync(string routingKey, byte[] body);
}