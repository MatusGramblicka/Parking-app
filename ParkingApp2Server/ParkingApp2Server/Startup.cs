using Entities;
using Entities.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ParkingApp2Server.Extensions;
using ParkingApp2Server.Middleware.WebSocket;
using ParkingApp2Server.Services;
using SlimBus.Client;
using SlimMessageBus;
using SlimMessageBus.Host;
using System;
using System.Collections.Generic;
using System.Text;
using WebHooks.Contracts;
using WebHooks.Infrastructure;
using WebHooks.Services;
using WebSocket;
using WebSocket.Contracts;
using WebSocket.Infrastructure;
using WebSocket.Middlewares;

namespace ParkingApp2Server;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureCors();
        services.ConfigureSqlContext(Configuration);
        services.ConfigureRepositoryManager();
        services.AddAutoMapper(typeof(Startup));

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<RepositoryContext>();

        var jwtSettings = Configuration.GetSection("JWTSettings");
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(jwtSettings["securityKey"]))
            };
        });

        services.Configure<JwtConfiguration>(Configuration.GetSection("JWTSettings"));
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.Configure<PriviledgedUsersConfiguration>(Configuration.GetSection("PriviledgedUsers"));

        services.AddControllers();
        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "ParkingApp", Version = "v1"}); });

        services.AddWebSocketConnections();

        services.AddSingleton<IHostedService, HeartbeatService>();

        services.AddHttpClient();

        services.AddTransient<IWebHookSender, WebHookSender>();
        services.AddSingleton<IWebHookCallManagerFactory, WebHookCallManagerFactory>();
        //services.AddSingleton<IWebHookSubscriptionRepository, WebHookSubscriptionRepository>();
        services.AddSingleton<IWebHookPayloadProcessor, WebHookPayloadProcessor>();
        services.AddScoped<IWebHookSubscriptionsProvider, WebHookSubscriptionsProvider>();

        services.AddScoped<IWebSocketSender, WebSocketSender>();

        ConfigureMessageBus(services);
        services.AddSingleton<WebHookMessageEventsBusAdapter>();

        services.AddRazorPages();
    }

    public void ConfigureMessageBus(IServiceCollection services)
    {
        services.AddSingleton<IMessagePublisher, MessageBusAdapter>();
        services.AddSingleton(BuildSlimMessageBus);
    }

    private IMessageBus BuildSlimMessageBus(IServiceProvider serviceProvider)
    {
        var builder = MessageBusBuildExtensions
            .CreateMemoryMessageBus()
            .WithDependencyResolver(serviceProvider)
            .AddProducer<WebHookMessage>("webhook")
            .AddConsumer<WebHookMessage, WebHookMessageEventsBusAdapter>("webhook");
        return builder.Build();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParkingApp2Server v1"));
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");

        var wsSettings = Configuration.GetSection("WebSocket");

        ITextWebSocketSubprotocol textWebSocketSubprotocol = new PlainTextWebSocketSubprotocol();
        var webSocketConnectionsOptions = new WebSocketConnectionsOptions
        {
            AllowedOrigins = new HashSet<string> {wsSettings["AllowedOrigins"]},
            SupportedSubProtocols = new List<ITextWebSocketSubprotocol>
            {
                new JsonWebSocketSubprotocol(),
                textWebSocketSubprotocol
            },
            DefaultSubProtocol = textWebSocketSubprotocol,
            SendSegmentSize = 4 * 1024
        };

        app.UseWebSockets(new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromSeconds(30)
        }).MapWebSocketConnections("/socket", webSocketConnectionsOptions);

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}