namespace FTPSearch.API.Application.Results.Paging;

public abstract record PagingFilterData
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}

public record PagingFilterData<TData>(TData Data) : PagingFilterData;