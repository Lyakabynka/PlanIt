using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace PlanIt.Identity.Application.Configurations;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtConfiguration _jwtConfiguration;
    public ConfigureJwtBearerOptions(IOptions<JwtConfiguration> jwtConfiguration) =>
        _jwtConfiguration = jwtConfiguration.Value;

    public void Configure(JwtBearerOptions options)
    {
        Configure(string.Empty, options);
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidAudience = _jwtConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key)),
            ClockSkew = TimeSpan.Zero,
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies[_jwtConfiguration.JwtCookieName];
                return Task.CompletedTask;
            }
        };
    }
}