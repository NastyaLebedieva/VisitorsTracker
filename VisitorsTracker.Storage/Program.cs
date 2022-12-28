using Common;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json")
    .Build();

var logPath = configuration["VisitorsLogLocation"];

//var factory = new ConnectionFactory()
//{
//    Uri = new Uri("amqp://guest:guest@localhost:5672")
//    //HostName = "localhost",
//    //UserName = "user",
//    //Password = "mypass",
//    ////do we need this variable?
//    //VirtualHost = "/"
//};

var factory = new ConnectionFactory();

configuration.GetSection("RabbitMqConnection").Bind(factory);

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "visitors",
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
        message.Date = DateTime.UtcNow;
        AppendLineToFileAsync(logPath, message.ToString());
    }
};

channel.BasicConsume(queue: "visitors",
                     autoAck: true,
                     consumer: consumer);

Console.ReadKey();

static void AppendLineToFileAsync(string path, string line)
{
    if (string.IsNullOrWhiteSpace(path))
        throw new ArgumentOutOfRangeException(nameof(path), path, "Was null or whitepsace.");

    if (!File.Exists(path))
        throw new FileNotFoundException("File not found.", nameof(path));

    using var file = File.Open(path, FileMode.Append, FileAccess.Write);
    using var writer = new StreamWriter(file);

    writer.WriteLine(line);
    writer.Flush();
}
