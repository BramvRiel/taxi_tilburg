using Microsoft.EntityFrameworkCore;
using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Database;

public class TaxiTilburgContext(DbContextOptions<TaxiTilburgContext> options) : DbContext(options)
{
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Traveler> Travelers => Set<Traveler>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<LocationDistance> LocationDistances => Set<LocationDistance>();
    public DbSet<LocationTravelTime> LocationTravelTimes => Set<LocationTravelTime>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LocationDistance>()
        .HasKey(e => new { e.StartingPointId, e.EndPointId });

        modelBuilder.Entity<LocationDistance>()
        .HasOne(e => e.StartingPoint)
        .WithMany(e => e.LocationDistanceTo)
        .HasForeignKey(e => e.StartingPointId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LocationDistance>()
        .HasOne(e => e.EndPoint)
        .WithMany(e => e.LocationDistanceFrom)
        .HasForeignKey(e => e.EndPointId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LocationTravelTime>()
        .HasKey(e => new { e.StartingPointId, e.EndPointId });

        modelBuilder.Entity<LocationTravelTime>()
        .HasOne(e => e.StartingPoint)
        .WithMany(e => e.LocationTravelTimesTo)
        .HasForeignKey(e => e.StartingPointId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LocationTravelTime>()
        .HasOne(e => e.EndPoint)
        .WithMany(e => e.LocationTravelTimesFrom)
        .HasForeignKey(e => e.EndPointId)
        .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}