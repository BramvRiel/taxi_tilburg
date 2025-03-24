using Microsoft.EntityFrameworkCore;
using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Database;

public class TaxiTilburgContext(DbContextOptions<TaxiTilburgContext> options) : DbContext(options)
{
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Traveler> Travelers => Set<Traveler>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<LocationConnection> LocationConnections => Set<LocationConnection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LocationConnection>()
        .HasKey(e => new { e.StartingPointId, e.EndPointId });

        modelBuilder.Entity<LocationConnection>()
        .HasOne(e => e.StartingPoint)
        .WithMany(e => e.LocationConnectionsTo)
        .HasForeignKey(e => e.StartingPointId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LocationConnection>()
        .HasOne(e => e.EndPoint)
        .WithMany(e => e.LocationConnectionsFrom)
        .HasForeignKey(e => e.EndPointId)
        .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}