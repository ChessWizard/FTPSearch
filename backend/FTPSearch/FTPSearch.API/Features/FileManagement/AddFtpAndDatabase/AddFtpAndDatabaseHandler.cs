using FluentFTP;
using FTPSearch.API.Application.Results;
using FTPSearch.API.Application.Results.Messages;
using FTPSearch.API.Application.Services;
using FTPSearch.API.Domain.Entities;
using FTPSearch.API.Domain.Enums;
using FTPSearch.API.Infrastructure.Configurations;
using FTPSearch.API.Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FTPSearch.API.Features.FileManagement.AddFtpAndDatabase;

public record AddFtpAndDatabaseCommand(IFormFileCollection? Files,
    string Directory,
    FileMetaType FileMetaType) : IRequest<Result<Unit>>;

public class AddFtpAndDatabaseCommandHandler(IFtpService ftpService,
    IOptions<FtpConfiguration> ftpConfiguration,
    FTPSearchDbContext dbContext) : IRequestHandler<AddFtpAndDatabaseCommand, Result<Unit>>
{
    private readonly FtpConfiguration _ftpConfiguration = ftpConfiguration.Value;
    
    public async Task<Result<Unit>> Handle(AddFtpAndDatabaseCommand request, CancellationToken cancellationToken)
    {
        List<FileEntity> files = [];
        switch (request.FileMetaType)
        {
            case FileMetaType.File:
                await AddFilesAsync(request, files, cancellationToken);
                break;
            case FileMetaType.Directory:
                await AddDirectoryAsync(request, files, cancellationToken);
                break;
        }

        dbContext.Files
            .AddRange(files);
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result > 0
            ? Result<Unit>.Success(Unit.Value, BusinessMessageConstants.Success.File.AddedFtpAndDatabase)
            : Result<Unit>.Error(BusinessMessageConstants.Error.File.TransferFromFtpToDatabaseFailed);
    }

    private async Task AddFilesAsync(AddFtpAndDatabaseCommand request, 
        List<FileEntity> files,
        CancellationToken cancellationToken)
    {
        foreach (var file in request.Files)
        {
            var isExistFile = await dbContext.Files
                .AnyAsync(existFile => existFile.Name == file.FileName &&
                                       existFile.Path == request.Directory, cancellationToken: cancellationToken);
            
            if(isExistFile) continue;
            
            var ftpStatusResult = await ftpService.AddFileAsync(file, 
                request.Directory,
                cancellationToken);

            if(!ftpStatusResult.IsSuccessful || ftpStatusResult.Data.FtpStatus is not FtpStatus.Success) continue;
            FileEntity onUploadFile = new()
            {
                Name = file.FileName,
                Path = request.Directory,
                Url = $"{_ftpConfiguration.Host}/{request.Directory.TrimStart('/')}",
                FileMetaType = request.FileMetaType
            };
            
            files.Add(onUploadFile);

            var createdDirectories = ftpStatusResult.Data.Directories;
            if (!createdDirectories.Any()) continue;
            AddIntoListCreatedDirectories(files, createdDirectories);
        }
    }

    private async Task AddDirectoryAsync(AddFtpAndDatabaseCommand request,
        List<FileEntity> files,
        CancellationToken cancellationToken)
    {
        var isExistDirectoryResult = await ftpService.AddDirectoriesAsync(request.Directory, cancellationToken);
        var createdDirectories = isExistDirectoryResult.Data;
        if (!isExistDirectoryResult.IsSuccessful ||
            createdDirectories.Count == 0) return;

        AddIntoListCreatedDirectories(files, createdDirectories);
    }

    private void AddIntoListCreatedDirectories(List<FileEntity> files,
        List<string> createdDirectories)
    {
        foreach (var directory in createdDirectories)
        {
            var fileParentDirectory = string.Join('/',
                directory.Split(['/'],
                        StringSplitOptions.RemoveEmptyEntries)
                    .Reverse()
                    .Skip(1)
                    .Reverse());
            
            FileEntity onUploadDirectory = new()
            {
                Name = directory!.Split('/').LastOrDefault()!,
                Path = fileParentDirectory,
                Url = $"{_ftpConfiguration.Host}/{directory.TrimStart('/')}",
                FileMetaType = FileMetaType.Directory
            };
            
            files.Add(onUploadDirectory);
        }
    }
}