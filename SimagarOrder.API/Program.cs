using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimagarOrder.API.Middleware;
using SimagarOrder.Application;
using SimagarOrder.Application.MediatR;
using SimagarOrder.Infrastructure.Persistence.Configurations;
using SimagarOrder.Infrastructure.Persistence.Repositories;
using SimagarOrder.Infrastructure.Services.MessageBus;
using SimagarOrder.Infrastructure.Services.Redis;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


// ------------------------------------------------------------
// ASP.NET Core Services
// ------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ------------------------------------------------------------
// Database
// ------------------------------------------------------------
builder.Services.AddDbContext<DbContextBasket>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<IUnitOfWork>(sp =>
    sp.GetRequiredService<DbContextBasket>());

builder.Services.AddScoped<IBasketRepository, BasketRepository>();


// ------------------------------------------------------------
// MediatR
// ------------------------------------------------------------
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
});

// Command / Query Dispatcher Wrappers
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();


// ------------------------------------------------------------
// FluentValidation
// ------------------------------------------------------------
builder.Services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(TransactionBehavior<,>));


// ------------------------------------------------------------
// RabbitMQ (MassTransit)
// ------------------------------------------------------------
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BasketEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            builder.Configuration["RabbitMq:Host"],
            h =>
            {
                h.Username(builder.Configuration["RabbitMq:Username"]!);
                h.Password(builder.Configuration["RabbitMq:Password"]!);
            });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped<IBasketEventPublisher, BasketEventPublisher>();


// ------------------------------------------------------------
// Redis
// ------------------------------------------------------------
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddScoped<IBasketCacheService, BasketCacheService>();


// ------------------------------------------------------------
// Hangfire (Currently Disabled)
// ------------------------------------------------------------
// builder.Services.AddHangfire(configuration => configuration
//     .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
//     .UseSimpleAssemblyNameTypeSerializer()
//     .UseRecommendedSerializerSettings()
//     .UseSqlServerStorage(
//         builder.Configuration.GetConnectionString("SqlServer")));

// builder.Services.AddHangfireServer();


// ------------------------------------------------------------
// Build Application
// ------------------------------------------------------------
var app = builder.Build();


// ------------------------------------------------------------
// HTTP Request Pipeline
// ------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();