using System.Text;

namespace SignalREmitter.Middleware;

public class BearerAuthenticationMiddleware
{
    private const string StaticToken = "secure-token";
    private readonly RequestDelegate _next;

    public BearerAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/secureChatHub"))
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader is null || !authHeader.StartsWith("Bearer"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Missing or invalid Authorization header");
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            if (token != StaticToken)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Invalid token");
                return;
            }
        }

        await _next(context);
    }
}
