namespace taxi_tilburg_backend.Models.Responses;

public class LocationItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<LocationConnectionItem> CanTravelTo { get; set; } = [];
}
