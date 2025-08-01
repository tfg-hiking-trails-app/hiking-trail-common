using System.ComponentModel.DataAnnotations;

namespace Common.API.DataAnnotations;

public class GuidValidator : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null || string.IsNullOrEmpty(value.ToString()))
            return true;
        
        return Guid.TryParse(value.ToString(), out _);
    }
}