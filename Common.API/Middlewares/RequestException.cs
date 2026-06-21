namespace Common.API.Middlewares;

public class RequestException
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string? Detail { get; set; }

    public RequestException(int statusCode, string message, string? detail)
    {
        StatusCode = statusCode;
        Message = message;
        Detail = detail;
    }
    
    public RequestException(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}