using BigBox_v4.Data;
using BigBox_v4;
using BigBox_v4.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;

namespace BigBox_v4.Middleware
{
    public class WebSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ApplicationDBContext db, IWebSessionRepository repo)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                if (!context.Request.Cookies.TryGetValue("WebSession", out var sessionId))
                {
                    await SignOut(context);
                    return;
                }

                await repo.RemoveExpiredSessionsAsync();

                var hash = SecurityHelpers.Hash(sessionId);
                var session = await repo.GetByHashAsync(hash);
                if (session == null || session.ExpiresAt < DateTime.UtcNow)
                {
                    await SignOut(context);
                    return;
                }
            }

            await _next(context);
        }

        private static async Task SignOut(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Cookies.Delete("WebSession");
            context.Response.Redirect("/Account/Login");
        }
    }

    public static class WebSessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSessions(this IApplicationBuilder builder)
            => builder.UseMiddleware<WebSessionMiddleware>();
    }
}
