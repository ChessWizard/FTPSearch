using FTPSearch.API.Domain.Entities.Base;
using FTPSearch.API.Domain.Entities.Base.Interfaces;
using FTPSearch.API.Domain.Enums;

namespace FTPSearch.API.Domain.Entities;

public class FileEntity : AuditEntity<Guid>
{
    public string Name { get; set; }

    public string Path { get; set; }

    public string Url { get; set; }

    public FileMetaType FileMetaType { get; set; }
}