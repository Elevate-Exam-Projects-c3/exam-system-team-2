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
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

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

            c.TagActionsBy(api =>
            {
                var tags = api.ActionDescriptor.EndpointMetadata.OfType<TagsAttribute>().FirstOrDefault()?.Tags;
                if (tags is { Count: > 0 })
                {
                    return tags.ToList();
                }

                var relativePath = api.RelativePath ?? string.Empty;
                if (relativePath.StartsWith("api/auth", StringComparison.OrdinalIgnoreCase))
                {
                    return new List<string> { "Authentication" };
                }

                var controllerName = api.ActionDescriptor.RouteValues["controller"];
                if (!string.IsNullOrEmpty(controllerName))
                {
                    return new List<string> { controllerName };
                }

                return new List<string> { "Other" };
            });

            c.DocumentFilter<SwaggerTagOrderDocumentFilter>();
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

public class SwaggerTagOrderDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var priorityTags = new List<string>
        {
            "Authentication",
            "Diplomas",
            "Quizzes"
        };

        var allTagNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Include priority tags
        foreach (var p in priorityTags)
        {
            allTagNames.Add(p);
        }

        // Collect tags from all paths/operations if any
        if (swaggerDoc.Paths != null)
        {
            foreach (var path in swaggerDoc.Paths.Values)
            {
                if (path.Operations != null)
                {
                    foreach (var operation in path.Operations.Values)
                    {
                        if (operation.Tags != null)
                        {
                            foreach (var tag in operation.Tags)
                            {
                                if (!string.IsNullOrWhiteSpace(tag.Name))
                                {
                                    allTagNames.Add(tag.Name);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Also include any existing tags in swaggerDoc.Tags
        if (swaggerDoc.Tags != null)
        {
            foreach (var tag in swaggerDoc.Tags)
            {
                if (!string.IsNullOrWhiteSpace(tag.Name))
                {
                    allTagNames.Add(tag.Name);
                }
            }
        }

        var comparer = Comparer<OpenApiTag>.Create((t1, t2) =>
        {
            if (t1 == null && t2 == null) return 0;
            if (t1 == null) return -1;
            if (t2 == null) return 1;

            var idx1 = priorityTags.FindIndex(p => p.Equals(t1.Name, StringComparison.OrdinalIgnoreCase));
            var idx2 = priorityTags.FindIndex(p => p.Equals(t2.Name, StringComparison.OrdinalIgnoreCase));

            if (idx1 >= 0 && idx2 >= 0) return idx1.CompareTo(idx2);
            if (idx1 >= 0) return -1;
            if (idx2 >= 0) return 1;

            return string.Compare(t1.Name, t2.Name, StringComparison.OrdinalIgnoreCase);
        });

        var sortedTags = new SortedSet<OpenApiTag>(comparer);
        foreach (var name in allTagNames)
        {
            sortedTags.Add(new OpenApiTag
            {
                Name = name,
                Description = name.Equals("Authentication", StringComparison.OrdinalIgnoreCase)
                    ? "Authentication and account management endpoints"
                    : null
            });
        }

        swaggerDoc.Tags = sortedTags;
    }
}
