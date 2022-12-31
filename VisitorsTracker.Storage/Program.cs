using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using VisitorsTracker.Common;
using VisitorsTracker.Storage;

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json")
    .Build();

var logPath = configuration.GetSection("VisitorsLogLocation").Value;

var factory = new ConnectionFactory();
configuration.GetSection("RabbitMq").Bind(factory);

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var queueName = configuration.GetSection("RabbitMq:Queue").Value;
channel.QueueDeclare(queue: queueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = JsonSerializer.Deserialize<Message>(body);

    if (message != null)
    {
        FileHelper.AppendLineToFileAsync(logPath, message.ToString());
    }
};

channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);

Console.ReadKey();
