using Microsoft.EntityFrameworkCore;
using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Database;

public class LocationRepository : Repository<Location>, ILocationRepository
{
    public LocationRepository(TaxiTilburgContext context) : base(context)
    {
    }

    public new async Task<List<Location>> GetAllAsync()
    {
        return await _dbSet
            .Include(l => l.LocationConnectionsFrom)
            .ToListAsync();
    }

    public new async Task<Location?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(l => l.LocationConnectionsFrom)
            .ThenInclude(c => c.StartingPoint)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
}