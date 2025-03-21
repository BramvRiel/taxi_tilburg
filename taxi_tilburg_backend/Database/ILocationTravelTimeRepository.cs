using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Database;

public interface ILocationTravelTimeRepository : IRepository<LocationTravelTime>
{
    Task<LocationTravelTime?> GetByIdAsync(int a, int b);
}
