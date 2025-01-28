using FTPSearch.API.Application.Results;
using FTPSearch.API.Application.Results.Messages;
using FTPSearch.API.Application.Services;
using FTPSearch.API.Domain.Entities;
using FTPSearch.API.Infrastructure.Data.Context;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FTPSearch.API.Features.FileManagement.TransferFromFtpToDatabase;

public record TransferFromFtpToDatabaseCommand : IRequest<Result<Unit>>;

public class TransferFromFtpToDatabaseCommandHandler(IFtpService ftpService,
    FTPSearchDbContext dbContext) : IRequestHandler<TransferFromFtpToDatabaseCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(TransferFromFtpToDatabaseCommand request, CancellationToken cancellationToken)
    {
        var filesFromFtpResult = await ftpService.GetAllAsync(cancellationToken);
        if (!filesFromFtpResult.IsSuccessful)
            return Result<Unit>.Error(filesFromFtpResult.Message);
        
        var fileEntities = filesFromFtpResult.Data.Adapt<List<FileEntity>>();

        var allFiles = await dbContext.Files
            .ToListAsync(cancellationToken);
        
        // TODO: Sorgu performanslı hale getirilmeli
        var existingFiles = allFiles
            .Where(file => fileEntities.Any(fileEntity => fileEntity.Name == file.Name))
            .ToList();

        var newFiles = fileEntities
            .Where(fe => !existingFiles
                .Any(ef => ef.Name == fe.Name && ef.Path == fe.Path))
            .ToList();
        
        dbContext.Files
            .AddRange(newFiles);
        
        var removalFiles = allFiles
            .Where(file => !fileEntities.Any(ftpFile => ftpFile.Name == file.Name &&
                                                        ftpFile.Path == file.Path))
            .ToList();
        
        dbContext.Files
            .RemoveRange(removalFiles);

        if (newFiles.Count == 0 && existingFiles.Count == 0)
            return Result<Unit>.Error(BusinessMessageConstants.Error.File.FtpFilesAlreadyExistOnDatabase);
         
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result > 0
            ? Result<Unit>.Success(Unit.Value, BusinessMessageConstants.Success.File.TransferFromFtpToDatabase)
            : Result<Unit>.Error(BusinessMessageConstants.Error.File.TransferFromFtpToDatabaseFailed);
    }
}