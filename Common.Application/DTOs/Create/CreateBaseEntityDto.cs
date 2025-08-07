namespace Common.Application.DTOs.Create;

public abstract record CreateBaseEntityDto
{
    public Guid? Code { get; set; }
}