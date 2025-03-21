using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Database;

public class LocationTravelTimeRepository : Repository<LocationTravelTime>, ILocationTravelTimeRepository
{
    public LocationTravelTimeRepository(TaxiTilburgContext context) : base(context)
    {
    }

    public async Task<LocationTravelTime?> GetByIdAsync(int a, int b)
    {
        return await _dbSet.FindAsync(a, b);
    }
}