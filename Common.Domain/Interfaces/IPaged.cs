namespace Common.Domain.Interfaces;

public interface IPaged<T>
{
    List<T> Content { get; }
    int PageNumber { get; }
    int PageSize { get; }
    int TotalCount { get; }
}