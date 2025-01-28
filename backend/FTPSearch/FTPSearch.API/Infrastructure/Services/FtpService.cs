using FluentFTP;
using FTPSearch.API.Application.Response;
using FTPSearch.API.Application.Results;
using FTPSearch.API.Application.Results.Messages;
using FTPSearch.API.Application.Services;
using FTPSearch.API.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace FTPSearch.API.Infrastructure.Services;

public partial class FtpService(IOptions<FtpConfiguration> ftpConfiguration) : IFtpService
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

    public async Task<Result<(FtpStatus FtpStatus, List<string> Directories)>> AddFileAsync(IFormFile file, 
        string directory,
        CancellationToken cancellationToken)
    {
        await using AsyncFtpClient ftpClient = new(_ftpConfiguration.Host, 
            _ftpConfiguration.Username,
            _ftpConfiguration.Password,
            _ftpConfiguration.Port);

        await ftpClient.Connect(cancellationToken);
        
        var createdDirectories = await CreateDirectoriesAsync(directory, ftpClient, cancellationToken);

        var onUploadFilePath = $"{directory}/{file.FileName}";      
        await using var fileStream = file.OpenReadStream();
        var uploadStatus = await ftpClient.UploadStream(fileStream, 
            onUploadFilePath, 
            token: cancellationToken);
        
        return Result<(FtpStatus FtpStatus, List<string> Directories)>.Success((uploadStatus, createdDirectories), BusinessMessageConstants.Success.Ftp.Added);
    }

    public async Task<Result<List<string>>> AddDirectoriesAsync(string directory, 
        CancellationToken cancellationToken)
    {
        await using AsyncFtpClient ftpClient = new(_ftpConfiguration.Host, 
            _ftpConfiguration.Username,
            _ftpConfiguration.Password,
            _ftpConfiguration.Port);

        await ftpClient.Connect(cancellationToken);
        
        var createdDirectories = await CreateDirectoriesAsync(directory, ftpClient, cancellationToken);
        
        return Result<List<string>>.Success(createdDirectories, BusinessMessageConstants.Success.Ftp.Added);
    }
}