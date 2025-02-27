using System.Text;
using Auctions.Database;
using Auctions.Database.Entities;
using Auctions.Features.Auth.Common.Options;
using Auctions.Features.Auth.Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Auctions.Extensions;

public static class AddJwtAuthExtension
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        
        services.Configure<JwtOptions>(configuration.GetRequiredSection(JwtOptions.SectionName));

        services.AddScoped<AuthTokenProcessor>();
        
        services.AddIdentity<User, IdentityRole<Guid>>(opts =>
        {
            opts.Password.RequireDigit = true;
            opts.Password.RequireLowercase = true;
            opts.Password.RequireNonAlphanumeric = true;
            opts.Password.RequireUppercase = true;
            opts.Password.RequiredLength = 8;
            opts.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<AppDbContext>();
        
        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opts => 
        {
            var jwtOptions = configuration
                .GetRequiredSection(JwtOptions.SectionName)
                .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
            };

            opts.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies[AuthTokenProcessor.AccessTokenCookieName];
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();
        return services;
    }
}