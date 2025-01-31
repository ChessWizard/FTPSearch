using FTPSearch.API.Application.Results;
using FTPSearch.API.Application.Results.Messages;
using FTPSearch.API.Application.Services;
using FTPSearch.API.Domain.Enums;
using FTPSearch.API.Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FTPSearch.API.Features.FileManagement.RemoveFtpAndDatabase;

public record RemoveFtpAndDatabaseCommand(Guid Id) : IRequest<Result<Unit>>;

public class RemoveFtpAndDatabaseCommandHandler(FTPSearchDbContext dbContext,
    IFtpService ftpService) : IRequestHandler<RemoveFtpAndDatabaseCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RemoveFtpAndDatabaseCommand request, CancellationToken cancellationToken)
    {
        var onRemoveFile = await dbContext.Files
            .FirstOrDefaultAsync(file => file.Id == request.Id && 
                                         file.FileMetaType == FileMetaType.File, cancellationToken);

        if (onRemoveFile is null)
            return Result<Unit>.Error(BusinessMessageConstants.Error.File.NotFoundOnRemove);

        var fileFullPath = $"{onRemoveFile.Path}/{onRemoveFile.Name}";

        var deleteFileFromFtpResult = await ftpService.DeleteFileAsync(fileFullPath, cancellationToken);

        if(!deleteFileFromFtpResult.IsSuccessful)
            return Result<Unit>.Error(deleteFileFromFtpResult.Message);
        
        dbContext.Files
            .Remove(onRemoveFile);

        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? Result<Unit>.Success(Unit.Value, BusinessMessageConstants.Success.File.RemovedFtpAndDatabase)
            : Result<Unit>.Error(BusinessMessageConstants.Error.File.RemovedFtpAndDatabaseFailed);
    }
}