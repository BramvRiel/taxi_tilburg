using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Services;

public interface IExcelImporter : IDisposable
{
    IExcelImporter FromStream(Stream stream);
    List<Models.Excel.LocationConnection> GetLocationDistances();
    List<Models.Excel.Location> GetLocations();
    List<Models.Excel.LocationConnection> GetLocationTravelTimes();
    List<Traveler> GetTravelers();
    List<Vehicle> GetVehicles();
}