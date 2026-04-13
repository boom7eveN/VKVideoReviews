using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using VKVideoReviews.WebApi.Settings;

namespace VKVideoReviews.WebApi.IoC;

public static class JwtAuthenticationConfigurator
{
    public static void ConfigureServices(IServiceCollection services, AppSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.JwtAuthSettings.Secret) ||
            Encoding.UTF8.GetByteCount(settings.JwtAuthSettings.Secret) < 32)
            throw new InvalidOperationException(
                "Jwt:Secret must be configured and at least 32 bytes (UTF-8) for HMAC-SHA256.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.JwtAuthSettings.Issuer,
                    ValidAudience = settings.JwtAuthSettings.Audience,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtAuthSettings.Secret)),
                    ClockSkew = TimeSpan.FromMinutes(1),
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        services.AddAuthorization();
    }
}