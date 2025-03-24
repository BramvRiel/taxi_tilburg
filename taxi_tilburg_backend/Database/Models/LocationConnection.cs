namespace taxi_tilburg_backend.Database.Models;

public class LocationConnection
{
    public required Location StartingPoint { get; set; }
    public int StartingPointId { get; set; }
    public int TravelTimeInSeconds { get; set; }
    public decimal DistanceInKilometers { get; set; }
    public required Location EndPoint { get; set; }
    public int EndPointId { get; set; }
}