using System.Text;
using System.Text.Json;
using Common.Domain.Interfaces.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Infrastructure.Messaging;

public abstract class AbstractRabbitMqQueueConsumer : IRabbitMqQueueConsumer
{
    private readonly IRabbitMqQueueProvider _channelProvider;
    
    public AbstractRabbitMqQueueConsumer(IRabbitMqQueueProvider channelProvider)
    {
        _channelProvider = channelProvider;
    }
    
    public abstract string QueueName { get; }
    public abstract string ExchangeName { get; }

    public async Task<T> BasicConsumeAsync<T>()
    {
        return await BasicConsumeAsync<T>(QueueName);
    }

    public async Task<T> BasicConsumeAsync<T>(string queueName)
    {
        TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
        
        IChannel channel = await _channelProvider.GetChannelAsync(ExchangeName, queueName);
        AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
            
                T? result = JsonSerializer.Deserialize<T>(message);

                if (result is null)
                    throw new Exception($"Error trying to deserialize the message: { message }");
                
                tcs.SetResult(result);
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }

            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);
        
        return await tcs.Task;
    }
    
}