using FTPSearch.API.Application.Results;
using FTPSearch.API.Application.Services;
using MediatR;

namespace FTPSearch.API.Features.FileManagement.Download;

public record DownloadCommand(string FilePath) : IRequest<Result<MemoryStream>>;

public class DownloadCommandHandler(IFtpService ftpService) : IRequestHandler<DownloadCommand, Result<MemoryStream>>
{
    public async Task<Result<MemoryStream>> Handle(DownloadCommand request, CancellationToken cancellationToken)
    {
        var downloadFileResult = await ftpService.DownloadFileAsync(request.FilePath, cancellationToken);
        return downloadFileResult;
    }
}