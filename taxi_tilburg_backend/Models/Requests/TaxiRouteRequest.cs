using System.ComponentModel;

namespace taxi_tilburg_backend.Models.Requests;

public class TaxiRouteRequest
{
    [DefaultValue(1)]
    public int StartingPointId { get; set; }
    [DefaultValue(new int[] { 2,3,4,5,6,7,8,9})]
    public int[] Stops { get; set; } = [];
    [DefaultValue(1)]
    public int? EndPointId { get; set; }
}