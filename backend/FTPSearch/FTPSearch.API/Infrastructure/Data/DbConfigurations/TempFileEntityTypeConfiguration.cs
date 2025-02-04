using FTPSearch.API.Domain.Entities;
using FTPSearch.API.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FTPSearch.API.Infrastructure.Data.DbConfigurations;

public class TempFileEntityTypeConfiguration : AuditEntityTypeConfiguration<TempFiles>
{
    public override void Configure(EntityTypeBuilder<TempFiles> builder)
    {
        builder.ToTable("TempFiles");
        
        builder.HasKey(file => file.Id);
        
        builder.HasIndex(e => e.Path)
            .HasDatabaseName("IX_TempFiles_Path");
        
        builder.HasIndex(e => new { e.Name, e.Path, e.SiteId})
            .IsUnique()
            .HasDatabaseName("IX_TempFiles_Name_Path");

        builder.Property(file => file.Name)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(file => file.Path)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(file => file.Url)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(file => file.FileMetaType)
            .IsRequired();
            
        builder.Property(file => file.SiteId)
            .IsRequired();
        
        base.Configure(builder);
    }
}