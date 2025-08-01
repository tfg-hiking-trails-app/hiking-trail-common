﻿namespace Common.Application.DTOs.Filter;

public record FilterEntityDto
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SortField { get; set; }
    public string? SortDirection { get; set; }
}