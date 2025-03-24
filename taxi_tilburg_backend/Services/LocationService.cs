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

        // Default route
        var route = new TaxiRoute();
        int distance = 0;
        int travelTime = 0;
        route.Stops.Add(new TaxiStop { Id = req.StartingPointId });
        foreach (var stop in req.Stops)
        {
            var connection = connections.FirstOrDefault(c =>
                c.StartingPointId == route.Stops.Last().Id
                && c.EndPointId == stop);
            if (connection is null)
            {
                continue;
            }
            distance += connection.DistanceInKilometers;
            travelTime += connection.TravelTimeInSeconds;
            route.Stops.Add(new TaxiStop
            {
                Id = stop,
                TotalDistance = distance,
                TotalTravelTime = travelTime
            });
        }
        if (req.EndPointId is not null)
        {
            var endPointId = req.EndPointId.Value;
            var connection = connections.FirstOrDefault(c =>
                c.StartingPointId == route.Stops.Last().Id
                && c.EndPointId == endPointId);
            if (connection is not null)
            {
                distance += connection.DistanceInKilometers;
                travelTime += connection.TravelTimeInSeconds;
                route.Stops.Add(new TaxiStop
                {
                    Id = req.EndPointId.Value,
                    TotalDistance = distance,
                    TotalTravelTime = travelTime
                });
            }
        }

        return route;
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
