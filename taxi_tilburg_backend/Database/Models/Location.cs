namespace taxi_tilburg_backend.Database.Models;

public class Location
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public List<LocationDistance> LocationDistanceTo { get; set; } = new();
    public List<LocationDistance> LocationDistanceFrom { get; set; } = new();
    public List<LocationTravelTime> LocationTravelTimesTo { get; set; } = new();
    public List<LocationTravelTime> LocationTravelTimesFrom { get; set; } = new();
}