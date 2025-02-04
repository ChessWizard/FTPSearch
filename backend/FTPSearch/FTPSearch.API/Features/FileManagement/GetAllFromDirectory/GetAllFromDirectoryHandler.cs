using FTPSearch.API.Application.Extensions;
using FTPSearch.API.Application.Response;
using FTPSearch.API.Application.Results.Messages;
using FTPSearch.API.Application.Results.Paging;
using FTPSearch.API.Infrastructure.Data.Context;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FTPSearch.API.Features.FileManagement.GetAllFromDirectory;

public record GetAllFromDirectoryQuery(string Directory, 
    int PageNumber,
    int PageSize) : IRequest<PagingResult<List<FileResponse>>>;

public class GetAllFromDirectoryQueryHandler(FTPSearchDbContext dbContext) : IRequestHandler<GetAllFromDirectoryQuery, PagingResult<List<FileResponse>>>
{
    public async Task<PagingResult<List<FileResponse>>> Handle(GetAllFromDirectoryQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Files
            .AsNoTracking()
            .Where(file => file.Path == request.Directory)
            .AsQueryable();
        
        var files = await query.Paginate(request.PageNumber, request.PageSize)
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