using FluentFTP;
using FTPSearch.API.Domain.Enums;

namespace FTPSearch.API.Infrastructure.Services;

public partial class FtpService
{
    private static FileMetaType GetFileMetaType(FtpObjectType ftpObjectType)
        => ftpObjectType switch
        {
            FtpObjectType.File => FileMetaType.File,
            FtpObjectType.Directory => FileMetaType.Directory,
            FtpObjectType.Link => FileMetaType.Link,
            _ => FileMetaType.File
        };

    private static async Task<List<string>> CreateDirectoriesAsync(string directory,
        AsyncFtpClient ftpClient,
        CancellationToken cancellationToken)
    {
        List<string> createdDirectories = new();
        var directories = directory.Split(['/'], StringSplitOptions.RemoveEmptyEntries);
        string currentPath = "";

        foreach (var directoryName in directories)
        {
            currentPath = string.IsNullOrEmpty(currentPath) ? directoryName : $"{currentPath}/{directoryName}";

            var isExistDirectory = await ftpClient.DirectoryExists(currentPath, cancellationToken);
            if (isExistDirectory) continue;

            var isDirectoryCreated = await ftpClient.CreateDirectory(currentPath, cancellationToken);
            if (!isDirectoryCreated) break;

            createdDirectories.Add(currentPath);
        }

        return createdDirectories;
    }
}