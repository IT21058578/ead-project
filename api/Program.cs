using api.Models;
using api.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// MongoDB configuration
var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? throw new InvalidOperationException("Database settings are required");
var client = new MongoClient(databaseSettings.ConnectionString);
var database = client.GetDatabase(databaseSettings.DatabaseName);

// Register the MongoDB context or direct collections
builder.Services.AddSingleton<IMongoDatabase>(database);

// Register the generic repository for dependency injection
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

// Add services to the container.
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

app.Run();