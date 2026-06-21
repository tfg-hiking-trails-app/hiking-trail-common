namespace Common.Domain.Filter;

public record FilterData
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortField { get; set; } = "id";
    public string? SortDirection { get; set; } = "asc";
}