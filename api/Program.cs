using System.Net;
using System.Net.Mail;
using System.Text.Json.Serialization;
using api.Configurations;
using api.Configuratons;
using api.Repositories;
using api.Services;
using Fluid;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load Configurations
var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? throw new InvalidOperationException("Database settings are required");
var mailSettings = builder.Configuration.GetSection("MailSettings").Get<MailSettings>() ?? throw new InvalidOperationException("Mail settings are required");

// Register Configuration
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

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
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<CredentialRepository>();
builder.Services.AddTransient<NotificationRepository>();
builder.Services.AddTransient<ReviewRepository>();

// Add Services
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