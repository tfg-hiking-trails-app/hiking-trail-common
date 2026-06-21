namespace Common.API.DTOs;

public abstract record BaseDto
{
    public Guid Code { get; set; }
    public DateTime CreatedAt { get; set; }
}