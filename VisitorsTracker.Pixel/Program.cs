using Pixel.Services;
using VisitorsTracker.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqConnection>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddScoped<IMessageProducer, MessageProducer>();

var app = builder.Build();

app.MapGet("/track", (HttpContext context, IWebHostEnvironment hostingEnvironment, IMessageProducer messageProducer) =>
{
    var clientIp = context.Connection.RemoteIpAddress?.ToString();

    if (!string.IsNullOrEmpty(clientIp))
    {
        messageProducer.SendMessage(new Message
        {
            Referrer = context.Request.Headers.Referer,
            UserAgent = context.Request.Headers.UserAgent,
            ClientIp = clientIp,
            Date = DateTime.UtcNow
        });
    }

    return Results.File(Path.Combine(hostingEnvironment.WebRootPath, "images", "pixel.gif"), contentType: "image/gif");
});

app.Run();
