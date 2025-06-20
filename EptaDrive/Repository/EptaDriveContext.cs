using System.Reflection;
using EptaDrive.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EptaDrive.Repository;

public class EptaDriveContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public DbSet<UserFile> UserFiles { get; set; }

    public EptaDriveContext(DbContextOptions<EptaDriveContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserFile>(entity =>
        {
            entity.HasKey(uf => uf.Id);
            entity.Property(uf => uf.Id).ValueGeneratedOnAdd();
        });
    }
}