using FTPSearch.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FTPSearch.API.Infrastructure.Data.DbConfigurations;

public class FileEntityTypeConfiguration : AuditEntityTypeConfiguration<FileEntity>
{
    public override void Configure(EntityTypeBuilder<FileEntity> builder)
    {
        builder.ToTable("Files");
        
        builder.HasKey(file => file.Id);
        
        builder.HasIndex(e => e.Path)
            .HasDatabaseName("IX_FileEntity_Path");
        
        builder.HasIndex(e => new { e.Name, e.Path })
            .IsUnique()
            .HasDatabaseName("IX_FileEntity_Name_Path");

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

        builder.HasIndex(file => file.Name)
            .HasDatabaseName("IX_FileEntity_Name_GIN")
            .HasMethod("GIN")
            .HasAnnotation("Npgsql:TsVectorConfig", "english");
        
        base.Configure(builder);
    }
}