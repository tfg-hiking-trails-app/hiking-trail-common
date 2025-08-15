using Common.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Common.API.Extensions;

public static class HttpRequestExtensions
{    
    public static string? GetUserCode(this HttpRequest request)
    {
        ITokenManager? tokenManager = request.HttpContext.RequestServices.GetService<ITokenManager>();
        
        if (tokenManager is null)
            return null;
        
        request.Headers.TryGetValue("Authorization", out var token);
        
        if (token.Count == 0 || string.IsNullOrEmpty(token[0]))
            return null;
        
        tokenManager
            .GetPayloadFromJwt(token[0]!.Substring("Bearer ".Length).Trim())
            .TryGetValue("userCode", out var userCode);
        
        return userCode?.ToString();
    }
}