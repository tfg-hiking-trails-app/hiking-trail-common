using RabbitMQ.Client;

namespace Common.Domain.Interfaces.Messaging;

public interface IRabbitMqConnectionProvider : IDisposable
{
    Task<IConnection> GetConnectionAsync();
}