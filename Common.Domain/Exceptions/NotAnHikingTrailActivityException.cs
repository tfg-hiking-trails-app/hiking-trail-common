namespace Common.Domain.Exceptions;

public class NotAnHikingTrailActivityException : Exception
{
    public NotAnHikingTrailActivityException(string? message = null)
        : base(message ?? "The uploaded activity is not a supported hiking-trail activity")
    {
    }
}
