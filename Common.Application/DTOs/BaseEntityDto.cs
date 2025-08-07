namespace Common.Application.DTOs;

public abstract record BaseEntityDto
{
    public int Id { get; set; }
    public Guid Code { get; set; }
}