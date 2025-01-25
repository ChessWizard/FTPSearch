using FTPSearch.API.Domain.Entities.Base.Interfaces;

namespace FTPSearch.API.Domain.Entities.Base;

public abstract class AuditEntity<T> : IAuditEntity<T>
{
    public T Id { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
    
    public virtual DateTimeOffset? ModifiedDate { get; set; }
    
    public virtual DateTimeOffset? DeletedDate { get; set; }
}