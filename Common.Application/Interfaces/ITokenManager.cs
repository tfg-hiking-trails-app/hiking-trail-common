namespace Common.Application.Interfaces;

public interface ITokenManager
{
    IDictionary<string, object> GetPayloadFromJwt(string token);
    
    string? GetUserCodeFromJwt(string token);
    
    string? GetUsernameFromJwt(string token);
}