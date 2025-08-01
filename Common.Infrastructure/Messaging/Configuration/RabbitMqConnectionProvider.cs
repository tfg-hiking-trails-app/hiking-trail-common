using Common.Domain.Interfaces.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
using RabbitMQ.Client;

namespace Common.Infrastructure.Messaging.Configuration;

public class RabbitMqConnectionProvider : IRabbitMqConnectionProvider
{
    private readonly ILogger<RabbitMqConnectionProvider> _logger;
    private readonly ConnectionFactory _connectionFactory;
    private IConnection? _connection;
    
    public RabbitMqConnectionProvider(ILogger<RabbitMqConnectionProvider> logger)
    {
        _logger = logger;
        _connectionFactory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "",
            Port = IntegerType.FromString(Environment.GetEnvironmentVariable("RABBITMQ_PORT")),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? "",
            Password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? "",
            AutomaticRecoveryEnabled = true
        };
    }
    
    public async void Dispose()
    {
        try
        {
            if (_connection is null)
                return;

            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Cannot dispose RabbitMq connection");
        }
    }

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_connection is null || !_connection.IsOpen)
            _connection = await _connectionFactory.CreateConnectionAsync();
        
        return _connection;
    }
    
}