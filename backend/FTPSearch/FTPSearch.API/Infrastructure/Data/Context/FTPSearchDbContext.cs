using FTPSearch.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FTPSearch.API.Infrastructure.Data.Context;

public class FTPSearchDbContext(DbContextOptions<FTPSearchDbContext> options) : DbContext(options)
{
    public DbSet<FileEntity> Files => Set<FileEntity>();
}