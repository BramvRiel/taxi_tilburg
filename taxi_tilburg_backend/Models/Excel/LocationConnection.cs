namespace taxi_tilburg_backend.Models.Excel;

public class LocationConnection
{
    public int StartingPointId { get; set; }
    public int? TravelTimeInSeconds { get; set; }
    public int? DistanceInKilometers { get; set; }
    public int EndPointId { get; set; }
}