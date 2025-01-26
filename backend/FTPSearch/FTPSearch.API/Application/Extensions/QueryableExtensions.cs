namespace FTPSearch.API.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pageNumber = 1, int pageSize = 20)
    {
        if(pageNumber < 1)
            pageNumber = 1;

        if (pageSize < 1)
            pageSize = 20;
        
        return source.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}