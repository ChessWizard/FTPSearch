using FTPSearch.API.Domain.Entities.Base.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FTPSearch.API.Infrastructure.Data.DbConfigurations;

public abstract class AuditEntityTypeConfiguration<T> 
    : IEntityTypeConfiguration<T> where T : 
    class, IAuditEntity<Guid>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(u => u.CreatedDate).IsRequired();
        builder.Property(u => u.ModifiedDate);
        builder.Property(u => u.DeletedDate);
        builder.Property(u => u.IsDeleted);
    }
}