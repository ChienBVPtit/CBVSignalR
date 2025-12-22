using CBVSignalR.Application.Interfaces;
using CBVSignalR.Application.Services;
using CBVSignalR.Context;
using CBVSignalR.Events.App.Consumers;
using CBVSignalR.Events.App.Runners;
using CBVSignalR.Events.Connections;
using CBVSignalR.Events.Interfaces;
using CBVSignalR.Hubs;
using CBVSignalR.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRCors", policy =>
    {
        //policy.WithOrigins("http://localhost:5139", "http://localhost:5500", "http://127.0.0.1:5500")
        policy
            .SetIsOriginAllowed(_ => true) // Cho phép tất cả origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add send email service
builder.Services.AddTransient<IGroupSubscriptionService, GroupSubscriptionService>();
builder.Services.AddTransient<IUserGroupSubscriptionService, UserGroupSubscriptionService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IInboxEventService, InboxEventService>();
builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();

// ================= Consumer - HostedService =================
builder.Services.AddSingleton<JoinUserToGroupConsumer>();
//builder.Services.AddHostedService<JoinUserToGroupConsumerHostedService>();

builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================= Authentication =================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Symmetric key validation
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), // Key phải giống hệt bên Identity

        ValidateIssuer = true,
        ValidIssuer = issuer, // "https://yourdomain.com"

        ValidateAudience = true,
        ValidAudience = audience, // "https://yourdomain.com"

        ValidateLifetime = true, // Kiểm tra token hết hạn
        ClockSkew = TimeSpan.FromMinutes(5) // Độ lệch thời gian chấp nhận được
    };

    // Rất quan trọng: SignalR WebSocket thường gửi token qua query string (access_token=...)
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // Chỉ áp dụng khi request là đến SignalR hub
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/hubs")) // Thay "/hubs" bằng prefix hub của bạn nếu khác
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ================= SignalR =================
builder.Services.AddSignalR();


// ================= Redis Backplane =================
//builder.Services.AddStackExchangeRedis(
//    builder.Configuration["Redis:ConnectionString"]);

//builder.Services.AddSignalR()
//    .AddStackExchangeRedis(builder.Configuration["Redis:ConnectionString"]);

var app = builder.Build();

app.UseCors("SignalRCors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationHub>("/hubs/notifications");

app.MapControllers();

app.Run();
