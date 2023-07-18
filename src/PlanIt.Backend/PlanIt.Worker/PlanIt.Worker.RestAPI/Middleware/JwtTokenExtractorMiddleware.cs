using PlanIt.Worker.Application.Configurations;

namespace PlanIt.Worker.RestAPI.Middleware;

public class JwtTokenExtractorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtConfiguration _configuration;

    public JwtTokenExtractorMiddleware(RequestDelegate next, JwtConfiguration configuration) =>
        (_next, _configuration) = (next, configuration);

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Cookies[_configuration.JwtCookieName];
        
        if (!string.IsNullOrEmpty(token))
            context.Request.Headers.Add("Authorization", $"Bearer {token}");
        
        await _next(context);
    }
}

public static class JwtTokenExtractorMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtTokenExtractor(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtTokenExtractorMiddleware>();
    }
}