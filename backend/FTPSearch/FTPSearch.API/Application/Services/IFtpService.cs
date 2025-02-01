using FluentFTP;
using FTPSearch.API.Application.Response;
using FTPSearch.API.Application.Results;
using FTPSearch.API.Infrastructure.Services;

namespace FTPSearch.API.Application.Services;

public interface IFtpService
{
    Task<Result<List<FileResponse>>> GetAllAsync(CancellationToken cancellationToken);

    Task<Result<(FtpStatus FtpStatus, List<string> Directories)>> AddFileAsync(IFormFile file, 
        string directory, 
        CancellationToken cancellationToken);
    
    Task<Result<List<string>>> AddDirectoriesAsync(string directory, 
        CancellationToken cancellationToken);

    Task<Result<FtpStatus>> DeleteFileAsync(string filePath,
        CancellationToken cancellationToken);
    
    Task<Result<FtpStatus>> DeleteDirectoryAsync(string directory,
        CancellationToken cancellationToken);
    
    Task<Result<MemoryStream>> DownloadFileAsync(string filePath,
        CancellationToken cancellationToken);
}