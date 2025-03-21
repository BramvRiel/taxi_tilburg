namespace taxi_tilburg_backend.Database.Models;

public class LocationTravelTime
{
    public Location StartingPoint { get; set; }
    public int StartingPointId { get; set; }
    public int TravelTimeInSeconds { get; set; }
    public Location EndPoint { get; set; }
    public int EndPointId { get; set; }
}