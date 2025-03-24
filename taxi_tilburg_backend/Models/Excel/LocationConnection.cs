namespace taxi_tilburg_backend.Models.Excel;

public class LocationConnection
{
    public int StartingPointId { get; set; }
    public int? TravelTimeInSeconds { get; set; }
    public decimal? DistanceInKilometers { get; set; }
    public int EndPointId { get; set; }
}