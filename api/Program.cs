using System.Net;
using System.Net.Mail;
using System.Text.Json.Serialization;
using api.Configurations;
using api.Configuratons;
using api.Identity;
using api.Models;
using api.Repositories;
using api.Services;
using Fluid;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load Configurations
var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? throw new InvalidOperationException("Database settings are required");
var mailSettings = builder.Configuration.GetSection("MailSettings").Get<MailSettings>() ?? throw new InvalidOperationException("Mail settings are required");
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new InvalidOperationException("JWT settings are required");

// Register Configuration
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Setup External Services
builder.Services.AddDbContext<AppDbContext>(options => options.UseMongoDB(databaseSettings.ConnectionString, databaseSettings.DatabaseName));
builder.Services.AddSingleton<FluidParser>();
builder.Services.AddFluentEmail(mailSettings.Username)
.AddLiquidRenderer()
.AddSmtpSender(() => new SmtpClient(mailSettings.Host, mailSettings.Port)
{
    EnableSsl = true,
    Credentials = new NetworkCredential(mailSettings.Username, mailSettings.Password)
});

// Setup Repositories
builder.Services.AddTransient<OrderRepository>();
builder.Services.AddTransient<ProductRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CredentialRepository>();
builder.Services.AddTransient<NotificationRepository>();
builder.Services.AddTransient<ReviewRepository>();

// Add Auth
builder.Services.AddDataProtection(); // Needed for AddDefaultTokenProviders to work 
builder.Services.AddIdentityCore<User>()
.AddUserStore<AppUserStore>()
.AddDefaultTokenProviders();

// Add Services
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ReviewService>();

// Add Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<ValidObjectIdFilter>();
    options.SchemaFilter<ValidEnumValueFilter>();
});

// Add Exception Handlers
builder.Services.AddProblemDetails();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
// app.UseAuthentication();
// app.UseAuthorization();

app.Run();