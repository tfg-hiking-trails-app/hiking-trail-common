using Common.Domain.Interfaces.Messaging;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Common.Infrastructure.Messaging.Configuration;

public class RabbitMqChannelProvider : IRabbitMqChannelProvider
{
    private readonly ILogger<RabbitMqChannelProvider> _logger;
    private readonly IRabbitMqConnectionProvider _connectionProvider;
    private IChannel? _channel;

    public RabbitMqChannelProvider(
        ILogger<RabbitMqChannelProvider> logger,
        IRabbitMqConnectionProvider connectionProvider)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
    
    public async void Dispose()
    {
        try
        {
            if (_channel is null)
                return;

            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Cannot dispose RabbitMq channel");
        }
    }

    public async Task<IChannel> GetChannelAsync()
    {
        if (_channel is null || _channel.IsClosed)
        {
            IConnection connection = await _connectionProvider.GetConnectionAsync();
            _channel = await connection.CreateChannelAsync();
        }
        
        return _channel;
    }
}