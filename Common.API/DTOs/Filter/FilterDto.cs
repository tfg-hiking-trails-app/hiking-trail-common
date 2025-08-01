using System.ComponentModel.DataAnnotations;

namespace Common.API.DTOs.Filter;

public record FilterDto
{
    public FilterDto(int pageNumber, int pageSize, string? sortField, string sortDirection)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SortField = sortField;
        SortDirection = sortDirection;
    }
    
    [Range(1, int.MaxValue, ErrorMessage = "The page number must be greater than or equal to 1.")]
    public int PageNumber { get; set; }

    [Range(1, 50, ErrorMessage = "The page size must be between 1 and 50")]
    public int PageSize { get; set; }

    public string? SortField { get; set; }
    public string? SortDirection { get; set; }
}