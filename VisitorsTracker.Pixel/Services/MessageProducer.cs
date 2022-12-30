using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using VisitorsTracker.Common;

namespace Pixel.Services;

public class MessageProducer : IMessageProducer
{
    private readonly RabbitMqConnection _connectionSettings;

    public MessageProducer(IOptions<RabbitMqConnection> options)
    {
        _connectionSettings = options.Value;
    }

    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            UserName = _connectionSettings.Username,
            Password = _connectionSettings.Password,
            HostName = _connectionSettings.Hostname,
            VirtualHost = _connectionSettings.VirtualHost,
            Port = _connectionSettings.Port
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _connectionSettings.Queue,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        channel.BasicPublish(exchange: "",
                             routingKey: _connectionSettings.Queue,
                             basicProperties: null,
                             body: body);
    }
}
