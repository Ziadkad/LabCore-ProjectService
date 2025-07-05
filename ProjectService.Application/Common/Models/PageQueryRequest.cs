namespace ProjectService.Application.Common.Models;

public record PageQueryRequest
{
    public int Page { get; set; } = 1;

    // public int Skip { get; set; } = 0;

    public int PageSize { get; set; } = 10;

    public string? SortColumn { get; set; }
    
    public bool SortAscending { get; set; } = true;
    
    // public int Offset => (Math.Max(1, Page) - 1) * PageSize;
}