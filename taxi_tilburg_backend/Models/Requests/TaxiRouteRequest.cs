namespace taxi_tilburg_backend.Models.Requests;

public class TaxiRouteRequest
{
    public int StartingPointId { get; set; }
    public int[] Stops { get; set; } = [];
    public int? EndPointId { get; set; }
}