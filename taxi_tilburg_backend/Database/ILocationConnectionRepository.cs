using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Database;

public interface ILocationConnectionRepository : IRepository<LocationConnection>
{
    Task<LocationConnection?> GetByIdAsync(int a, int b);
}
