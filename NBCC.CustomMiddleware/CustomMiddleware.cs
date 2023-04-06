using Microsoft.AspNetCore.Http;

namespace NBCC.WebApplicaion;

public class CustomMiddleware
{
    RequestDelegate Next { get; }

    public CustomMiddleware(RequestDelegate next) => Next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        await Next(httpContext);
    }
}
