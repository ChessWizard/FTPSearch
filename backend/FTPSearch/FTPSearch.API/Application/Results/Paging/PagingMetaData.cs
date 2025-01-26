namespace FTPSearch.API.Application.Results.Paging;

public class PagingMetaData
{
    public int PageSize { get; set; }

    private int CurrentPage { get; set; }

    private int TotalPages { get; set; }

    public int TotalCount { get; set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;

    public PagingMetaData(int pageSize, int currentPage, int totalCount)
    {
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}