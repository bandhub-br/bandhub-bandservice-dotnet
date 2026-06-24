using BandHub.BandService.Features.Bands.Domain;
using Microsoft.EntityFrameworkCore;

namespace BandHub.BandService.Infrastructure.Persistence;

public class BandDbContext : DbContext
{
    public BandDbContext(DbContextOptions<BandDbContext> options)
        : base(options)
    {
    }

    public DbSet<Band> Bands => Set<Band>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<Band>(entity =>
        {
            entity.ToTable("bands");

            entity.HasKey(x => x.Id);

            entity.HasKey(x => x.AccountId);

            entity.HasIndex(x => x.AccountId)
                .IsUnique();

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Genre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.SpotifyId)
                .HasMaxLength(100);

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });
    }
}
