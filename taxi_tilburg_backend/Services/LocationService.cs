using taxi_tilburg_backend.Database;
using taxi_tilburg_backend.Models.Excel;
using taxi_tilburg_backend.Models.Requests;
using taxi_tilburg_backend.Models.Responses;

namespace taxi_tilburg_backend.Services;

internal class LocationService : ILocationService
{
    private readonly ILocationRepository locationRepository;
    private readonly ILocationConnectionRepository connectionRepository;

    public LocationService(
        ILocationRepository locationRepository,
        ILocationConnectionRepository connectionRepository)
    {
        this.locationRepository = locationRepository;
        this.connectionRepository = connectionRepository;
    }

    public async Task AddConnectionsAsync(List<LocationConnection> parsedConnections)
    {
        foreach (var parsedConnection in parsedConnections)
        {
            var location = await locationRepository.GetByIdAsync(parsedConnection.StartingPointId);
            if (location is null)
            {
                return;
            }
            var endPoint = await locationRepository.GetByIdAsync(parsedConnection.EndPointId);
            if (endPoint is null)
            {
                return;
            }
            var connection = await connectionRepository.GetByIdAsync(parsedConnection.StartingPointId, parsedConnection.EndPointId);
            if (connection is null)
            {
                connection = new Database.Models.LocationConnection
                {
                    StartingPoint = location,
                    EndPoint = endPoint
                };
                await connectionRepository.AddAsync(connection);
            }
            if (parsedConnection.DistanceInKilometers.HasValue)
            {
                connection.DistanceInKilometers = parsedConnection.DistanceInKilometers.Value;
            }
            if (parsedConnection.TravelTimeInSeconds.HasValue)
            {
                connection.TravelTimeInSeconds = parsedConnection.TravelTimeInSeconds.Value;
            }
            await connectionRepository.UpdateAsync(connection);
        }
    }

    public void AddLocation(List<Location> locations)
    {
        locations.ForEach(async l =>
        {
            if (await locationRepository.GetByIdAsync(l.Id) is null)
            {
                await locationRepository.AddAsync(new Database.Models.Location
                {
                    Id = l.Id,
                    Name = l.Name,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                });
            }
        });
    }

    public async Task<TaxiRoute?> CalculateRouteAsync(TaxiRouteRequest req)
    {
        var connections = await connectionRepository.GetAllAsync();
        if (connections.Count == 0)
        {
            throw new ArgumentException("No connections found. Upload not yet completed.");
        }

        return GetOptimalRoute(req, connections);
    }

    private static TaxiRoute? GetOptimalRoute(TaxiRouteRequest req, List<Database.Models.LocationConnection> connections)
    {
        var routes = new List<TaxiRoute>();
        var route = new TaxiRoute();
        route.Stops.Add(new TaxiStop { Id = req.StartingPointId });

        GetAllRoutes(req.Stops, route, connections, routes);

        if (req.EndPointId is not null)
        {
            for (var i = 0; i < routes.Count; i++)
            {
                routes[i] = AddStopToRoute(req.EndPointId.Value, routes[i], connections);
            }
        }

        return routes.GroupBy(r => r.Stops.Last().TotalDistance).OrderBy(g => g.Key).FirstOrDefault()?.FirstOrDefault();
    }

    private static void GetAllRoutes(int[] stops, TaxiRoute route, List<Database.Models.LocationConnection> connections, List<TaxiRoute> routes)
    {
        if (route.Stops.Count == stops.Length + 1)
        {
            routes.Add(new TaxiRoute { Stops = [.. route.Stops] });
            return;
        }

        foreach (var stop in stops)
        {
            if (!route.Stops.Select(s => s.Id).Contains(stop))
            {
                var newRoute = AddStopToRoute(stop, route, connections);
                GetAllRoutes(stops, newRoute, connections, routes);
            }
        }
    }

    private static TaxiRoute AddStopToRoute(int stop, TaxiRoute route, List<Database.Models.LocationConnection> connections)
    {
        var newRoute = new TaxiRoute
        {
            Stops = [.. route.Stops]
        };
        var connection = connections.FirstOrDefault(c =>
                c.StartingPointId == route.Stops.Last().Id
                && c.EndPointId == stop) ?? throw new InvalidOperationException("No connection can be made to designated stop");
        newRoute.Stops.Add(new TaxiStop
        {
            Id = stop,
            TotalDistance = newRoute.Stops.Last().TotalDistance + connection.DistanceInKilometers,
            TotalTravelTime = newRoute.Stops.Last().TotalTravelTime + connection.TravelTimeInSeconds
        });
        return newRoute;
    }

    public async Task<LocationItem?> GetLocationAsync(int id)
    {
        var location = await locationRepository.GetByIdAsync(id);
        if (location is null)
        {
            return null;
        }

        return new LocationItem
        {
            Id = location.Id,
            Name = location.Name,
            Connections = [.. location.LocationConnectionsFrom.Select(con => new LocationConnectionItem {
                Id = con.StartingPoint.Id,
                Name = con.StartingPoint.Name,
                DistanceInKilometers = con.DistanceInKilometers,
                TravelTimeInSeconds = con.TravelTimeInSeconds
             })]
        };
    }

    public async Task<List<LocationListItem>> ListLocations()
    {
        var locations = await locationRepository.GetAllAsync();
        return [.. locations.Select(l => new LocationListItem
        {
            Id = l.Id,
            Name = l.Name,
            Latitude = l.Latitude,
            Longitude = l.Longitude,
        })];
    }
}
