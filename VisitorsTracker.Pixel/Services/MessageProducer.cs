using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Pixel.Services;

public class MessageProducer : IMessageProducer
{
    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "visitors", 
                             durable: true, 
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        channel.BasicPublish(exchange: "",
                             routingKey: "visitors",
                             basicProperties: null,
                             body: body);
    }
}
