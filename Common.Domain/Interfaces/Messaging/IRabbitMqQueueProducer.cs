namespace Common.Domain.Interfaces.Messaging;

public interface IRabbitMqQueueProducer
{
    string QueueName { get; }
    string ExchangeName { get; }
    Task BasicPublishAsync(byte[] body);
}