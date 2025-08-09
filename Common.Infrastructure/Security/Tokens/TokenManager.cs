using System.IdentityModel.Tokens.Jwt;
using Common.Application.Interfaces;

namespace Common.Infrastructure.Security.Tokens;

public class TokenManager : ITokenManager
{
    public IDictionary<string, object> GetPayloadFromJwt(string token)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        
        return handler.ReadJwtToken(token).Payload;
    }
}