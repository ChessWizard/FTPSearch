using FTPSearch.API.Domain.Entities.Base;
using FTPSearch.API.Domain.Entities.Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FTPSearch.API.Infrastructure.Data.Interceptors;

public partial class AuditEntitySaveChangeInterceptor
{
    private void UpdateAuidtFields(DbContext context)
    {
        var entries = context.ChangeTracker
            .Entries<IAuditEntity>()
            .Where(e => e.State is EntityState.Added or 
                                                         EntityState.Modified or 
                                                         EntityState.Deleted);
        
        var currentTime = DateTimeOffset.UtcNow;

        foreach (var entry in entries)
        {
            var auditEntity = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                    auditEntity.CreatedDate = currentTime;
                    break;
                case EntityState.Deleted:
                    auditEntity.ModifiedDate = currentTime;
                    break;
                case EntityState.Modified:
                    entry.State = EntityState.Modified;
                    auditEntity.DeletedDate = currentTime;
                    auditEntity.IsDeleted = true;
                    break;
            }
        }
    }
}