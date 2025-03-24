namespace taxi_tilburg_backend.Database.Models;

public class Location
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public List<LocationConnection> LocationConnectionsTo { get; set; } = [];
    public List<LocationConnection> LocationConnectionsFrom { get; set; } = [];
}