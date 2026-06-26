using FluentValidation;
using Hangfire;
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// - - - - 
builder.Services.AddDbContext<DbContextBasket>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<
        ApplicationAssemblyReference>();
});
builder.Services.AddScoped<IUnitOfWork>(sp =>
    sp.GetRequiredService<DbContextBasket>());
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

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

//redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration);
});

//builder.Services.AddHangfire(configuration => configuration
//           .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
//           .UseSimpleAssemblyNameTypeSerializer()
//           .UseRecommendedSerializerSettings()
//           .UseSqlServerStorage(builder.Configuration.GetConnectionString("SqlServer")));
//builder.Services.AddHangfireServer();

builder.Services.AddScoped<IBasketCacheService, BasketCacheService>();
//MediatR Wrapper
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

builder.Services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(TransactionBehavior<,>));
builder.Services.AddScoped<IBasketEventPublisher, BasketEventPublisher>();
var app = builder.Build();

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
