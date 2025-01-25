using FTPSearch.API.Application.Response;
using FTPSearch.API.Application.Results;

namespace FTPSearch.API.Application.Services;

public interface IFtpService
{
    Task<Result<List<FileResponse>>> GetAllAsync(CancellationToken cancellationToken);

    Task<Result<List<FileResponse>>> GetAllRelatedGivenDirectory(string path, 
        CancellationToken cancellationToken);
}