
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SearchExplorer.Application.Commands;
using SearchExplorer.Application.Handlers;
using SearchExplorer.Core.Interfaces;
using SearchExplorer.Infrastructure;
using SearchExplorer.Api.Middleware;
using SearchExplorer.Infrastructure;
using SearchExplorer.Application.Services;
using SearchExplorer.Core.Interfaces;
using SearchExplorer.Core.Entities;
using SearchExplorer.Infrastructure.Repositories;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Setup logging
var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory); 
}

// Setup Serilog for file logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(Path.Combine(logDirectory, "log.txt"), rollingInterval: RollingInterval.Day)  // Log to file with daily rolling
    .CreateLogger();

var connectionString = builder.Configuration
    .GetSection("SearchExplorerApiSettings")
    .GetValue<string>("ConnectionString");

// Add DbContext using SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Logging.AddSerilog();  

// JWT settings
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

// Register MediatR
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(
        typeof(ProductSearchHandler).Assembly,
        typeof(LoginCommand).Assembly // Register from both assemblies in one call
    );
});

// Register the repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<JwtTokenGenerator>();

// Authentication setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

// Swagger setup
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Search Explorer API",
        Version = "v1",
        Description = "API for SearchExplorer"
    });
});

// Register controllers
builder.Services.AddControllers();

var app = builder.Build();  // This is where the application is actually built.
app.UseSwagger();

// Configure Swagger UI
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Search Explorer API v1");
    c.RoutePrefix = "swagger";
});

// Configure middleware pipeline
app.UseMiddleware<RequestResponseLoggingMiddleware>(); // Enable request/response logging
// app.UseMiddleware<GlobalExceptionMiddleware>(); // Uncomment if you want global exception handling
app.UseHttpsRedirection();

app.UseAuthentication(); // Ensure Authentication Middleware is added
app.UseAuthorization();

app.MapControllers();

app.Run();
