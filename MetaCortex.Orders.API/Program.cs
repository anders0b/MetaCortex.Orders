using MetaCortex.Orders.API.BackgroundServices;
using MetaCortex.Orders.API.Extensions;
using MetaCortex.Orders.API.InterfaceM;
using MetaCortex.Orders.API.Services;
using MetaCortex.Orders.DataAcess;
using MetaCortex.Orders.DataAcess.Entities;
using MetaCortex.Orders.DataAcess.MessageBroker;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient($"mongodb://{settings.Host}:{settings.Port}");
});

builder.Services.AddSingleton<IOrderRepository, OrderRepository>();


builder.Services.AddSingleton(sp => new RabbitMqConfiguration()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
});

builder.Services.AddSingleton<ObjectConverterService>();
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
builder.Services.AddSingleton<IMessageProducerService, MessageProducerService>();
builder.Services.AddSingleton<IMessageConsumerService, MessageConsumerService>();

builder.Services.AddHostedService<MessageConsumerHostedService>();


var app = builder.Build();

app.MapOrderEndpoints();

app.Run();
