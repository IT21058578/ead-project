using api.Configurations;
using api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MongoDB-EFCore configuration
var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? throw new InvalidOperationException("Database settings are required");
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddDbContext<AppDbContext>(options => options.UseMongoDB(databaseSettings.ConnectionString, databaseSettings.DatabaseName));

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