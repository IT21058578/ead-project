using api.Configurations;
using api.Models;
using api.Repositories;
using api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MongoDB-EFCore configuration
var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? throw new InvalidOperationException("Database settings are required");
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddDbContext<AppDbContext>(options => options.UseMongoDB(databaseSettings.ConnectionString, databaseSettings.DatabaseName));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Setup Repositories
builder.Services.AddTransient<OrderRepository>();
builder.Services.AddTransient<ProductRepository>();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<CredentialRepository>();
builder.Services.AddTransient<NotificationRepository>();
builder.Services.AddTransient<ReviewRepository>();

// Add Services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ReviewService>();

// Add Controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Exception Handlers
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();