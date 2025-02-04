using FTPSearch.API.Domain.Entities;
using FTPSearch.API.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace FTPSearch.API.Infrastructure.Data.Context;

public class FTPSearchDbContext(DbContextOptions<FTPSearchDbContext> options) : DbContext(options)
{
    public DbSet<FileEntity> Files => Set<FileEntity>();
    public DbSet<TempFiles> TempFiles => Set<TempFiles>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}