using FTPSearch.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FTPSearch.API.Infrastructure.Data.Context;

public class FTPSearchDbContext(DbContextOptions<FTPSearchDbContext> options) : DbContext(options)
{
    public DbSet<FileEntity> Files => Set<FileEntity>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}