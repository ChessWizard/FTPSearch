using FTPSearch.API.Domain.Entities.Base;
using FTPSearch.API.Domain.Entities.Base.Interfaces;

namespace FTPSearch.API.Domain.Entities;

public class FileEntity : AuditEntity<Guid>, ISoftDeleteEntity
{
    public string Name { get; set; }

    public string Path { get; set; }

    public string Url { get; set; }
    
    public bool IsDeleted { get; set; }
}