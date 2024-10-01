using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json.Serialization;
using api.Configurations;
using api.Configuratons;
using api.Models;
using api.Repositories;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Load Configurations
var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? throw new InvalidOperationException("Database settings are required");
var mailSettings = builder.Configuration.GetSection("MailSettings").Get<MailSettings>() ?? throw new InvalidOperationException("Mail settings are required");
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new InvalidOperationException("JWT settings are required");

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Register Configuration
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Setup External Services
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.WithOrigins("*")
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
builder.Services.AddHttpLogging(o => { });
builder.Services.AddDbContext<AppDbContext>(options => options.UseMongoDB(databaseSettings.ConnectionString, databaseSettings.DatabaseName));
builder.Services.AddFluentEmail(mailSettings.Username)
.AddRazorRenderer()
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
builder.Services.AddTransient<TokenRepository>();

// Add Auth-Identity Configurations
builder.Services.AddDataProtection(); // Needed for AddDefaultTokenProviders to work 
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
})
.AddUserStore<AppUserStore>()
.AddSignInManager<SignInManager<User>>()
.AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = false,
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessSecret)),
    };
});

// Add Services
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();

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

app.UseHttpLogging();
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.Run();