using FTPSearch.API.Application.Response;
using FTPSearch.API.Application.Results;
using FTPSearch.API.Application.Services;
using MediatR;

namespace FTPSearch.API.Features.FileManagement.GetAll;

public record GetAllQuery : IRequest<Result<List<FileResponse>>>;

public class GetAllQueryHandler(IFtpService ftpService) : IRequestHandler<GetAllQuery, Result<List<FileResponse>>>
{
    public async Task<Result<List<FileResponse>>> Handle(GetAllQuery query, CancellationToken cancellationToken)
    {
        var response = await ftpService.GetAllAsync(1, cancellationToken);
        return response;
    }
}