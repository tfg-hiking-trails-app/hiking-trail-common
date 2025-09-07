namespace Common.Application.Utils;

public static class Validator
{
    public static readonly HashSet<string> ValidContentTypeImage = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "image/png",
        "image/jpg",
        "image/jpeg",
    };
    
    public static readonly HashSet<string> ValidExtensionsImage = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".png",
        ".jpg",
        ".jpeg"
    };
    
    public static void CheckGuid(Guid code)
    {
        if (code == Guid.Empty)
            throw new ArgumentNullException(nameof(code));
    }
    
    public static void CheckNullArgument(string? argument, string field)
    {
        if (string.IsNullOrEmpty(argument))
            throw new ArgumentNullException(field);
    }
    
    public static void CheckNullArgument(object? argument, string field)
    {
        if (argument is null)
            throw new ArgumentNullException(field);
    }

    public static void CheckPositiveValue(double value, string field)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(field);
    }

    public static void CheckPositiveValue(int value, string field)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(field);
    }
    
}