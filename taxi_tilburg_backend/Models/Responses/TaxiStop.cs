namespace taxi_tilburg_backend.Models.Responses;

public class TaxiStop
{
    public int Id { get; set; }
    public decimal TotalDistance { get; set; }
    public int TotalTravelTime { get; set; }
}