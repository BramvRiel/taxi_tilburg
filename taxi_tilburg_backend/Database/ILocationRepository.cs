using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Database;

public interface ILocationRepository : IRepository<Location>
{
    new Task<List<Location>> GetAllAsync();
}
