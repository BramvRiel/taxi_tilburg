namespace taxi_tilburg_backend.Models.Responses;

public class LocationListItem
{
    public int Id { get; internal set; }
    public string? Name { get; internal set; }
    public decimal Latitude { get; internal set; }
    public decimal Longitude { get; internal set; }
}