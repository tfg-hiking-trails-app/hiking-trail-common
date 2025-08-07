namespace Common.API.DTOs.Create;

public abstract record CreateBaseDto
{
    public Guid? Code { get; set; }
}