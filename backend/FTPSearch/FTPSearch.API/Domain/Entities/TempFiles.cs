using FTPSearch.API.Domain.Enums;

namespace FTPSearch.API.Domain.Entities.Base;

public class TempFiles: AuditEntity<Guid>
{
    public string Name { get; set; }

    public string Path { get; set; }

    public string Url { get; set; }
    
    public int SiteId { get; set; }

    public FileMetaType FileMetaType { get; set; }
}