using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Database;

public class LocationDistanceRepository : Repository<LocationDistance>, ILocationDistanceRepository
{
    public LocationDistanceRepository(TaxiTilburgContext context) : base(context)
    {
    }

    public async Task<LocationDistance?> GetByIdAsync(int a, int b)
    {
        return await _dbSet.FindAsync(a, b);
    }
}