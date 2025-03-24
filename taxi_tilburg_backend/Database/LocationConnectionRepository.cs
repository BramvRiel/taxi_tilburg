using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Database;

public class LocationConnectionRepository : Repository<LocationConnection>, ILocationConnectionRepository
{
    public LocationConnectionRepository(TaxiTilburgContext context) : base(context)
    {
    }

    public async Task<LocationConnection?> GetByIdAsync(int a, int b)
    {
        return await _dbSet.FindAsync(a, b);
    }
}