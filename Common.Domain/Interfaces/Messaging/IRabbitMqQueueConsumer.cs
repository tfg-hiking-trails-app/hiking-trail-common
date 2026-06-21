namespace Common.Domain.Interfaces.Messaging;

public interface IRabbitMqQueueConsumer
{
    string QueueName { get; }
    string ExchangeName { get; }
    Task<T> BasicConsumeAsync<T>();
    Task<T> BasicConsumeAsync<T>(string queueName);
}