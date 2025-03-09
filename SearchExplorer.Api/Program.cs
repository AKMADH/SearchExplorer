using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SearchExplorer.Application.Commands;
using SearchExplorer.Application.Handlers;
using SearchExplorer.Application.Services;
using SearchExplorer.Core.Interfaces;
using SearchExplorer.Infrastructure;
using SearchExplorer.Infrastructure.Data;
using SearchExplorer.Infrastructure.Middleware;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(typeof(ProductSearchQueryHandler).Assembly);
});
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(typeof(LoginCommand).Assembly);
});

// Register the repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();



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

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Search Explorer API",
//        Version = "v1",
//        Description = "API for searching products in Search Explorer.",
//        Contact = new OpenApiContact
//        {
//            Name = "Your Name",
//            Email = "your.email@example.com",
//            Url = new Uri("https://yourwebsite.com")
//        }
//    });

//    // Add JWT Authentication to Swagger UI
//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Enter 'Bearer' [space] and then your token in the text input below."
//    });

//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            new string[] {}
//        }
//    });
//});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Mohre API",
        Version = "v1",
        Description = "API for generating and retrieving access tokens"
    });
});
// Register the IAuthService with its implementation (AuthService)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<JwtTokenGenerator>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();

var app = builder.Build();  // This is where the application is actually built.
app.UseSwagger();

// Now you can configure the middleware pipeline
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Search Explorer API v1");
    c.RoutePrefix = "swagger";
});

// Configure additional middleware (should be done after app.Build())
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication(); // Ensure Authentication Middleware is added
app.UseAuthorization();

app.MapControllers();

app.Run();
