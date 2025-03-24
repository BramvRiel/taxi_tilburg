using taxi_tilburg_backend.Models.Requests;
using taxi_tilburg_backend.Models.Responses;

namespace taxi_tilburg_backend.Services;

interface ILocationService
{
    Task<List<LocationListItem>> ListLocations();
    void AddLocation(List<Models.Excel.Location> locations);
    Task AddConnectionsAsync(List<Models.Excel.LocationConnection> locationDistances);
    Task<LocationItem?> GetLocationAsync(int id);
    Task<TaxiRoute?> CalculateRouteAsync(TaxiRouteRequest req);
}
