using FluentFTP;
using FTPSearch.API.Application.Response;
using FTPSearch.API.Application.Results;
using FTPSearch.API.Application.Results.Messages;
using FTPSearch.API.Application.Services;
using FTPSearch.API.Infrastructure.Configurations;
using FTPSearch.API.Infrastructure.Data.Context;
using Microsoft.Extensions.Options;

namespace FTPSearch.API.Infrastructure.Services;

public partial class FtpService(FTPSearchDbContext dbContext,
    IOptions<FtpConfiguration> ftpConfiguration) : IFtpService
{
    private readonly FtpConfiguration _ftpConfiguration = ftpConfiguration.Value;

    public async Task<Result<List<FileResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using AsyncFtpClient ftpClient = new(_ftpConfiguration.Host, 
            _ftpConfiguration.Username,
            _ftpConfiguration.Password,
            _ftpConfiguration.Port);

        await ftpClient.Connect(cancellationToken);
        var filesWithSubDirectories = await ftpClient.GetListing(_ftpConfiguration.BasePath,
            FtpListOption.Recursive, cancellationToken);

        if (filesWithSubDirectories is null || filesWithSubDirectories.Length == 0)
            return Result<List<FileResponse>>.Error(BusinessMessageConstants.Error.Ftp.NotFound);

        var fileResponses = filesWithSubDirectories
            .Select(file => new FileResponse(
                file.Name, 
                 Path.GetDirectoryName(file.FullName!)!.TrimStart('/'), 
                $"{_ftpConfiguration.Host}/{file.FullName.TrimStart('/')}",
                GetFileMetaType(file.Type)
            ))
            .ToList();
        
        return Result<List<FileResponse>>.Success(fileResponses, BusinessMessageConstants.Success.Ftp.AllFound);
    }

    public Task<Result<List<FileResponse>>> GetAllRelatedGivenDirectory(string path, 
        CancellationToken cancellationToken)
    {
        return null;
    }
}