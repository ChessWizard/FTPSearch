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
}