using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using Pixel.Services;
using RabbitMQ.Client;
using System.IO;
using System.Text.Json;
using System.Text;
using IHostEnvironment = Microsoft.Extensions.Hosting.IHostEnvironment;
using Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMessageProducer, MessageProducer>();

var app = builder.Build();

app.MapGet("/track", (HttpContext context, IHostEnvironment hostingEnvironment, IMessageProducer messageProducer) =>
{
    var referrer = context.Request.Headers.Referer;
    var userAgent = context.Request.Headers.UserAgent;
    var clientIp = context.Connection.RemoteIpAddress?.ToString();

    if (!string.IsNullOrEmpty(clientIp))
    {
        messageProducer.SendMessage(new Message
        {
            Referrer = referrer,
            UserAgent = userAgent,
            ClientIp = clientIp
        });
    }

    return Results.File(hostingEnvironment.ContentRootPath + @"wwwroot/images/pixel.gif", contentType: "image/gif");
});

app.UseStaticFiles();

app.Run();
