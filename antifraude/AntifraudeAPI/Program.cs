using Confluent.Kafka;
using Domain.Consumers;
using Domain.Domain;
using Domain.Events;
using Domain.Handlers;
using Domain.Infrastructure;
using Domain.Producers;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Producers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;
using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Dispatchers;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Mapped the register to save en MongoDB database
BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<TrxtCreatedEvent>();
BsonClassMap.RegisterClassMap<TrxtUpdatedEvent>();

// Sqlserver Database Connection
Action<DbContextOptionsBuilder> configureDbContext = (o => o.UseLazyLoadingProxies().UseSqlServer(
    builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

// Persistence all operations with Journal Table 
builder.Services.AddScoped<IJournalRepository, JournalRepository>();
// 
builder.Services.AddHostedService<HostedService>();

// Create database and tables execute with sa user 
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dataContext.Database.EnsureCreated(); //-> only sa user

// MongoDB Connection
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));

// Kafka Connection Producer
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
// Kafka Connection Consumer
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
// Kafka Consumer and Producer
builder.Services.AddScoped<IEventConsumer, EventConsumer>();
builder.Services.AddScoped<IEventProducer, EventProducer>();

// CQRS, event sourcing and relfexion by type transaction and store sql server
// Create, Update and Delete Operations
builder.Services.AddScoped<IEventHandlers, EventHandlers>();

// register query handler methods 
var dispatcher = new QueryDispatcher();
builder.Services.AddSingleton<IQueryDispatcher<JournalEntity>>(_ => dispatcher);

// persitence in MongoDB  
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
// store mongo and kafka
builder.Services.AddScoped<IEventStore, EventStore>();
// event sourcing
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
