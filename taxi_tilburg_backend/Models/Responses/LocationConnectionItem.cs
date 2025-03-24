namespace taxi_tilburg_backend.Models.Responses;

public class LocationConnectionItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int TravelTimeInSeconds { get; set; }
    public decimal DistanceInKilometers { get; set; }
}