namespace Common.API.DTOs.Update;

public abstract record UpdateBaseDto
{
    public Guid? Code { get; set; }
}