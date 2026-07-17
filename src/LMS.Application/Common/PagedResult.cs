namespace LMS.Application.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public static PagedResult<T> Create(IEnumerable<T> items, int totalItems, int currentPage, int pageSize)
    {
        return new PagedResult<T>
        {
            Items = items,
            TotalItems = totalItems,
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }
}
