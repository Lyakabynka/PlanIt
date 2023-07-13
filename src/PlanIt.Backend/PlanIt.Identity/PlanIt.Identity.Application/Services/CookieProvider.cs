using Microsoft.AspNetCore.Http;
using PlanIt.Identity.Application.Configurations;

namespace PlanIt.Identity.Application.Services;

public class CookieProvider
{
    private readonly RefreshSessionConfiguration _refreshSessionConfiguration;
    private readonly JwtConfiguration _jwtConfiguration;
    
    public CookieProvider(
        RefreshSessionConfiguration refreshSessionConfiguration,
        JwtConfiguration jwtConfiguration) =>
        (_refreshSessionConfiguration,_jwtConfiguration) = (refreshSessionConfiguration,jwtConfiguration);
    
    // Insert refresh token into response http-only cookie
    public void AddRefreshCookieToResponse(HttpResponse response, string value)
    {
        response.Cookies.Append(_refreshSessionConfiguration.RefreshCookieName, value,
            new CookieOptions
            {
                HttpOnly = true, 
                SameSite = SameSiteMode.Lax,
                Expires = new DateTimeOffset(DateTime.UtcNow.AddHours(_refreshSessionConfiguration.HoursToExpiration))
            });
    }
    
    public void AddJwtCookieToResponse(HttpResponse response, string value)
    {
        response.Cookies.Append(_jwtConfiguration.JwtCookieName, value,
            new CookieOptions
            {
                HttpOnly = true, 
                SameSite = SameSiteMode.Lax,
                Expires = new DateTimeOffset(DateTime.UtcNow.AddMinutes(_jwtConfiguration.MinutesToExpiration))
            });
    }

    // Extract refresh token from http-only cookie
    public Guid? GetRefreshTokenFromCookie(HttpRequest request)
    {
        // Try to extract refresh token from cookie. If it is absent - exception
        var refreshTokenString = request.Cookies[_refreshSessionConfiguration.RefreshCookieName];

        return Guid.TryParse(refreshTokenString, out var refreshToken) 
            ? refreshToken 
            : null;
    }

    public void DeleteJwtTokenFromCookies(HttpResponse response)
    {
        response.Cookies.Delete(_jwtConfiguration.JwtCookieName);
    }

    public void DeleteRefreshTokenFromCookies(HttpResponse response)
    {
        response.Cookies.Delete(_refreshSessionConfiguration.RefreshCookieName);
    }
}
