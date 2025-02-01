using FTPSearch.API.Application.Results;
using FTPSearch.API.Application.Results.Messages;
using FTPSearch.API.Application.Services;
using FTPSearch.API.Domain.Entities;
using FTPSearch.API.Domain.Enums;
using FTPSearch.API.Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FTPSearch.API.Features.FileManagement.RemoveFtpAndDatabase;

public record RemoveFtpAndDatabaseCommand(Guid Id,
    FileMetaType FileMetaType) : IRequest<Result<Unit>>;

public class RemoveFtpAndDatabaseCommandHandler(FTPSearchDbContext dbContext,
    IFtpService ftpService) : IRequestHandler<RemoveFtpAndDatabaseCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RemoveFtpAndDatabaseCommand request, CancellationToken cancellationToken)
    {
        var onRemoveFile = await dbContext.Files
            .FirstOrDefaultAsync(file => file.Id == request.Id && 
                                         file.FileMetaType == request.FileMetaType, cancellationToken);

        if (onRemoveFile is null)
            return Result<Unit>.Error(BusinessMessageConstants.Error.File.NotFoundOnRemove);
        
        var fullPath = $"{onRemoveFile.Path}/{onRemoveFile.Name}";
        
        switch (request.FileMetaType)
        {
            case FileMetaType.File:
                var removeFileResult = await RemoveFileAsync(fullPath, cancellationToken);
                if(removeFileResult is 
                   { IsSuccessful: false, Message.HttpStatus: StatusCodes.Status500InternalServerError }) 
                    return removeFileResult;
                
                dbContext.Files
                    .Remove(onRemoveFile);
                
                break;
                
            case FileMetaType.Directory:
                var removeDirectoryResult = await RemoveDirectoryAsync(fullPath, cancellationToken);
                if(removeDirectoryResult is 
                   { IsSuccessful: false, Message.HttpStatus: StatusCodes.Status500InternalServerError }) 
                    return removeDirectoryResult;

                var onRemoveChildFiles = await GetChildFilesAndDirectories(fullPath, cancellationToken);

                if (onRemoveChildFiles.Count is 0)
                {
                    dbContext.Files
                        .Remove(onRemoveFile);
                }
                else
                {
                    onRemoveChildFiles.Add(onRemoveFile);
                    dbContext.Files
                        .RemoveRange(onRemoveChildFiles);
                }

                break;    
        }
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? Result<Unit>.Success(Unit.Value, BusinessMessageConstants.Success.File.RemovedFtpAndDatabase)
            : Result<Unit>.Error(BusinessMessageConstants.Error.File.RemovedFtpAndDatabaseFailed);
    }

    private async Task<Result<Unit>> RemoveFileAsync(string fileFullPath,
        CancellationToken cancellationToken)
    {
        var deleteFileFromFtpResult = await ftpService.DeleteFileAsync(fileFullPath, cancellationToken);
        return !deleteFileFromFtpResult.IsSuccessful ? Result<Unit>.Error(deleteFileFromFtpResult.Message) 
                                                     : Result<Unit>.Success(Unit.Value, null);
    }

    private async Task<Result<Unit>> RemoveDirectoryAsync(string fileFullPath,
        CancellationToken cancellationToken)
    {
        var deleteDirectoryFromFtpResult = await ftpService.DeleteDirectoryAsync(fileFullPath, cancellationToken);
        return !deleteDirectoryFromFtpResult.IsSuccessful ? Result<Unit>.Error(deleteDirectoryFromFtpResult.Message) 
                                                          : Result<Unit>.Success(Unit.Value, null);
    }

    private async Task<List<FileEntity>> GetChildFilesAndDirectories(string fullPath,
        CancellationToken cancellationToken)
    {
        var childFilesAndDirectories = await dbContext.Files
            .Where(file => file.Path
                .StartsWith(fullPath))
            .ToListAsync(cancellationToken);
        
        return childFilesAndDirectories;
    }
}