namespace Common.Application;

public record FileEntityDto
{
    public string? ContentType { get; set; }
    public string? ContentDisposition { get; set; }
    public long Length { get; set; }
    public string? Name { get; set; }
    public string? FileName { get; set; }
    public byte[] Content { get; set; } = [];
}