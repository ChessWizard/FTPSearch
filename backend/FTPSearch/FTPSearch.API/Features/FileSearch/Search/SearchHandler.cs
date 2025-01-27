using FTPSearch.API.Application.Extensions;
using FTPSearch.API.Application.Response;
using FTPSearch.API.Application.Results;
using FTPSearch.API.Application.Results.Messages;
using FTPSearch.API.Application.Results.Paging;
using FTPSearch.API.Infrastructure.Data.Context;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FTPSearch.API.Features.FileSearch.Search;

public record SearchQuery(string SearchKeyword, 
    int PageNumber,
    int PageSize) : IRequest<PagingResult<List<FileResponse>>>;

public class SearchQueryHandler(FTPSearchDbContext dbContext) : IRequestHandler<SearchQuery, PagingResult<List<FileResponse>>>
{
    public async Task<PagingResult<List<FileResponse>>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Files
            .Where(file => EF.Functions.ToTsVector("english", file.Name)
                .Matches(EF.Functions.ToTsQuery("english", $"{request.SearchKeyword}:*"))
            || EF.Functions.Like(file.Name, $"%{request.SearchKeyword}%"));
        
        var files = await query
            .Paginate(request.PageNumber, request.PageSize)
            .ToListAsync(cancellationToken);
        
        if (files is null || !files.Any())
            return PagingResult<List<FileResponse>>.Error(BusinessMessageConstants.Error.File.NotFoundByDirectory);
        
        var result = files.Adapt<List<FileResponse>>();
        var totalCount = await query.CountAsync(cancellationToken);
        PagingMetaData pagingMetaData = new(
            request.PageSize,
            request.PageNumber,
            totalCount
        );
        
        return PagingResult<List<FileResponse>>.Success(result, 
            pagingMetaData,
            BusinessMessageConstants.Success.File.FoundByDirectory);
    }
}