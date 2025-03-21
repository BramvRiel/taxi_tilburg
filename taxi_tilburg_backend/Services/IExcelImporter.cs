using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Services;

public interface IExcelImporter : IDisposable
{
    IExcelImporter FromStream(Stream stream);
    List<LocationDistance> GetLocationDistances();
    List<Location> GetLocations();
    List<LocationTravelTime> GetLocationTravelTimes();
    List<Traveler> GetTravelers();
    List<Vehicle> GetVehicles();
}