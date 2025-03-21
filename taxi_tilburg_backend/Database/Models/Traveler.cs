namespace taxi_tilburg_backend.Database.Models;

public class Traveler
{
    public int Id { get; internal set; }
    public required string Name { get; set; }
    public bool Wheelchair { get; set; }
}