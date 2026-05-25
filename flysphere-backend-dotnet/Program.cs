using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FlysphereBackendDotnet.Data;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // ✅ Prevent circular reference serialization crash
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Register Repositories
builder.Services.AddScoped<FlysphereBackendDotnet.Repositories.IUserRepository, FlysphereBackendDotnet.Repositories.UserRepository>();
builder.Services.AddScoped<FlysphereBackendDotnet.Repositories.IFlightRepository, FlysphereBackendDotnet.Repositories.FlightRepository>();
builder.Services.AddScoped<FlysphereBackendDotnet.Repositories.IBookingRepository, FlysphereBackendDotnet.Repositories.BookingRepository>();
builder.Services.AddScoped<FlysphereBackendDotnet.Repositories.IPassengerRepository, FlysphereBackendDotnet.Repositories.PassengerRepository>();
builder.Services.AddScoped<FlysphereBackendDotnet.Repositories.IBookingSegmentRepository, FlysphereBackendDotnet.Repositories.BookingSegmentRepository>();

 // Register Services
builder.Services.AddScoped<FlysphereBackendDotnet.Services.IAuthService, FlysphereBackendDotnet.Services.AuthService>();
builder.Services.AddScoped<FlysphereBackendDotnet.Services.IFlightService, FlysphereBackendDotnet.Services.FlightService>();
builder.Services.AddScoped<FlysphereBackendDotnet.Services.IBookingService, FlysphereBackendDotnet.Services.BookingService>();
builder.Services.AddScoped<FlysphereBackendDotnet.Services.ITicketService, FlysphereBackendDotnet.Services.TicketService>();

// Configure CORS (Allow Angular Frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Configure PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        RoleClaimType = System.Security.Claims.ClaimTypes.Role,
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Global Exception Middleware
app.UseMiddleware<FlysphereBackendDotnet.Middleware.ExceptionMiddleware>();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Log actual listening URLs on startup
app.Lifetime.ApplicationStarted.Register(() =>
{
    foreach (var url in app.Urls)
    {
        Console.WriteLine($"Now listening on: {url}");
    }
});

app.Run();
