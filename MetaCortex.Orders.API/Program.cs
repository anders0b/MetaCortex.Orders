using MetaCortex.Orders.API.Extensions;
using MetaCortex.Orders.DataAccess.Entities;
using MetaCortex.Orders.DataAcess;
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

var app = builder.Build();

app.MapOrderEndpoints();

app.Run();
