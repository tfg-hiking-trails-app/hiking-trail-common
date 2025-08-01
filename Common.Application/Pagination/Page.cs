using Common.Domain.Interfaces;

namespace Common.Application.Pagination;

public class Page<T> : IPaged<T>
{
    public Page(
        List<T> content,
        int pageNumber,
        int pageSize,
        int totalCount
    )
    {
        Content = content;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);
    }
    
    public List<T> Content { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}