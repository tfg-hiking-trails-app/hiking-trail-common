namespace Common.Application.Interfaces;

public interface ITokenManager
{
    IDictionary<string, object> GetPayloadFromJwt(string token);
}