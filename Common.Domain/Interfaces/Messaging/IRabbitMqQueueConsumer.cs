namespace Common.Domain.Interfaces.Messaging;

public interface IRabbitMqQueueConsumer
{
    Task<T> BasicConsumeAsync<T>(string routingKey);
}