using System.Collections.Concurrent;
using FTPSearch.API.Application.Results;
using FTPSearch.API.Application.Results.Messages;
using FTPSearch.API.Application.Services;
using FTPSearch.API.Domain.Entities;
using FTPSearch.API.Domain.Entities.Base;
using FTPSearch.API.Infrastructure.Data.Context;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FTPSearch.API.Features.FileManagement.TransferFromFtpToDatabase;

public record TransferFromFtpToDatabaseCommand(int SiteId) : IRequest<Result<Unit>>;

public class TransferFromFtpToDatabaseCommandHandler(
    IFtpService ftpService,
    FTPSearchDbContext dbContext) : IRequestHandler<TransferFromFtpToDatabaseCommand, Result<Unit>>
{
    private static readonly ConcurrentDictionary<int, bool> LockedSites = new();
    public async Task<Result<Unit>> Handle(TransferFromFtpToDatabaseCommand request,
        CancellationToken cancellationToken)
    {
        var targetSiteId = request.SiteId;
        var lockAcquired = LockedSites.TryAdd(targetSiteId, true);
        if (!lockAcquired)
        {
            return Result<Unit>.Error(BusinessMessageConstants.Error.File.FtpFilesAlreadyExistOnDatabase);
        }
        var ftpResult = await ftpService.GetAllAsync(targetSiteId, cancellationToken: cancellationToken);
        if (!ftpResult.IsSuccessful)
            return Result<Unit>.Error(ftpResult.Message);

        var tempFilesList = ftpResult.Data.Adapt<List<TempFiles>>();

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken: cancellationToken);
        try
        {
            if (tempFilesList.Count == 0)
                return Result<Unit>.Error(BusinessMessageConstants.Error.File.FtpFilesAlreadyExistOnDatabase);

            await dbContext.TempFiles.AddRangeAsync(tempFilesList, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var newFiles = await dbContext.TempFiles
                .FromSqlInterpolated($@"
                    SELECT t.*
                    FROM ""TempFiles"" t
                    LEFT JOIN ""Files"" f 
                      ON t.""Name"" = f.""Name""
                      AND t.""Path"" = f.""Path""
                      AND t.""SiteId"" = {targetSiteId}
                      AND f.""SiteId"" = {targetSiteId}
                    WHERE f.""Name"" IS NULL")
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);

            var deletedFiles = await dbContext.Files
                .FromSqlInterpolated($@"
                    SELECT f.*
                    FROM ""Files"" f
                    LEFT JOIN ""TempFiles"" t 
                      ON f.""Name"" = t.""Name""
                      AND f.""Path"" = t.""Path""
                      AND f.""SiteId"" = {targetSiteId}
                    WHERE t.""Name"" IS NULL
                    AND f.""SiteId"" = {targetSiteId}
                    AND f.""IsDeleted"" IS FALSE")
                .ToListAsync(cancellationToken: cancellationToken);

            if (newFiles.Count == 0 && deletedFiles.Count == 0)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result<Unit>.Error(BusinessMessageConstants.Error.File.FtpFilesAlreadyExistOnDatabase);
            }

            if (newFiles.Count > 0)
            {
                var filesToAdd = newFiles.Adapt<List<FileEntity>>();
                await dbContext.Files.AddRangeAsync(filesToAdd, cancellationToken);
            }

            if (deletedFiles.Count > 0)
                dbContext.Files.RemoveRange(deletedFiles);

            await dbContext.Database.ExecuteSqlInterpolatedAsync(
                $@"DELETE FROM ""TempFiles"" 
                WHERE ""SiteId"" = {targetSiteId};", cancellationToken);

            var changes = await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return changes > 0
                ? Result<Unit>.Success(Unit.Value, BusinessMessageConstants.Success.File.TransferFromFtpToDatabase)
                : Result<Unit>.Error(BusinessMessageConstants.Error.File.TransferFromFtpToDatabaseFailed);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            return Result<Unit>.Error(BusinessMessageConstants.Error.File.TransferFromFtpToDatabaseFailed);
        }
        finally
        {
            LockedSites.TryRemove(targetSiteId, out _);
        }

    }
}