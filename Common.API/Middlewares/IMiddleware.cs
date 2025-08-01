using Microsoft.AspNetCore.Http;

namespace Common.API.Middlewares;

public interface IMiddleware
{
    Task InvokeAsync(HttpContext context);
}