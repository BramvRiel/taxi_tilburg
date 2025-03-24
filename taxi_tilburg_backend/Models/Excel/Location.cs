namespace taxi_tilburg_backend.Models.Excel;

public class Location
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}