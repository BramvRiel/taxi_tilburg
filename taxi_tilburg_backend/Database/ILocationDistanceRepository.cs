using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Database;

public interface ILocationDistanceRepository : IRepository<LocationDistance>
{
    Task<LocationDistance?> GetByIdAsync(int a, int b);
}
