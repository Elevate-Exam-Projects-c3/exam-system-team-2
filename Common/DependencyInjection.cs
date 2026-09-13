using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using exam_system.Common.Services;
using exam_system.Common.Services.Interfaces;
using exam_system.Features.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace exam_system.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "Examination System API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.ParameterLocation.Header,
                Description = "Enter your JWT Bearer token"
            });

            c.AddSecurityRequirement(_ => new Microsoft.OpenApi.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", null, null),
                    new List<string>()
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = ClaimTypes.Role
            };

            options.Events = new JwtBearerEvents
            {
                OnForbidden = async context =>
                {
                    if (!context.Response.HasStarted)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        var response = EndpointResponse<object>.Fail(
                            "Forbidden: You do not have permission to access this resource.",
                            StatusCodes.Status403Forbidden);
                        await context.Response.WriteAsJsonAsync(response);
                    }
                },
                OnChallenge = async context =>
                {
                    if (!context.Response.HasStarted)
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var response = EndpointResponse<object>.Fail(
                            "Unauthorized: Invalid or expired token.",
                            StatusCodes.Status401Unauthorized);
                        await context.Response.WriteAsJsonAsync(response);
                    }
                }
            };
        });

        return services;
    }

    public static IServiceCollection AddAuthRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddFixedWindowLimiter("auth-rate-limit", opt =>
            {
                opt.PermitLimit = 10;
                opt.Window = TimeSpan.FromMinutes(1);
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = 0;
            });
        });

        return services;
    }
}
