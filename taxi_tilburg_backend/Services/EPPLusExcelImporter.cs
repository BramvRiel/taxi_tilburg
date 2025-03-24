using System.Globalization;
using OfficeOpenXml;
using taxi_tilburg_backend.Database.Models;

namespace taxi_tilburg_backend.Services;

public class EPPlusExcelImporter : IExcelImporter
{
    private ExcelPackage? _package;

    public void Dispose()
    {
        if (_package is not null)
        {
            _package.Dispose();
        }
    }

    public IExcelImporter FromStream(Stream stream)
    {
        // EPPlus requires this setting for non-commercial use
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        _package = new ExcelPackage(stream);

        return this;
    }

    public List<Models.Excel.LocationConnection> GetLocationDistances()
    {
        if (_package is null)
        {
            throw new ArgumentNullException("Package not initiated.");
        }
        var result = new List<Models.Excel.LocationConnection>();
        ExcelWorksheet worksheet = _package.Workbook.Worksheets[0]; // First sheet
        const int LocationDistancesTitle = 2;
        const int LocationDistancesRowStart = 3;
        int rowCount = LocationDistancesRowStart + 10;
        const int LocationDistanceColStart = 6;
        int colCount = LocationDistanceColStart + 10;
        for (int row = LocationDistancesRowStart; row <= rowCount; row++)
        {
            var aId = worksheet.Cells[row, LocationDistanceColStart].Text;
            if (!int.TryParse(aId, CultureInfo.CurrentCulture, out var locationAId))
            {
                continue;
            }
            for (int col = LocationDistanceColStart; col <= colCount; col++)
            {
                var distance = worksheet.Cells[row, col].Text;
                if (!int.TryParse(distance, NumberStyles.AllowThousands, CultureInfo.GetCultureInfo("de"), out var distanceInKm))
                {
                    continue;
                }

                var bId = worksheet.Cells[LocationDistancesTitle, col].Text;
                if (!int.TryParse(bId, CultureInfo.CurrentCulture, out var locationBId))
                {
                    continue;
                }

                result.Add(new Models.Excel.LocationConnection
                {
                    StartingPointId = locationAId,
                    DistanceInKilometers = distanceInKm,
                    EndPointId = locationBId
                });
            }
        }

        return result;
    }

    public List<Models.Excel.Location> GetLocations()
    {
        if (_package is null)
        {
            throw new ArgumentNullException("Package not initiated.");
        }
        var result = new List<Models.Excel.Location>();
        ExcelWorksheet worksheet = _package.Workbook.Worksheets[0]; // First sheet
        const int LocationRowStart = 3;
        int rowCount = LocationRowStart + 10;
        const int ColId = 1;
        const int ColName = 2;
        const int ColLatitude = 3;
        const int ColLongitude = 4;
        for (int row = LocationRowStart; row <= rowCount; row++)
        {

            var id = worksheet.Cells[row, ColId].Text;
            if (!int.TryParse(id, CultureInfo.CurrentCulture, out var locationId))
            {
                continue;
            }
            var locationName = worksheet.Cells[row, ColName].Text;

            var latitude = worksheet.Cells[row, ColLatitude].Text;
            if (!decimal.TryParse(latitude, CultureInfo.CurrentCulture, out var locationLatitude))
            {
                continue;
            }
            var longitude = worksheet.Cells[row, ColLongitude].Text;
            if (!decimal.TryParse(longitude, CultureInfo.CurrentCulture, out var locationLongitude))
            {
                continue;
            }

            result.Add(new Models.Excel.Location
            {
                Id = locationId,
                Name = locationName,
                Latitude = locationLatitude,
                Longitude = locationLongitude
            });
        }

        return result;
    }

    public List<Models.Excel.LocationConnection> GetLocationTravelTimes()
    {
        if (_package is null)
        {
            throw new ArgumentNullException("Package not initiated.");
        }
        var result = new List<Models.Excel.LocationConnection>();
        ExcelWorksheet worksheet = _package.Workbook.Worksheets[0]; // First sheet
        const int LocationTravelTimesTitle = 16;
        const int LocationTravelTimesRowStart = 17;
        int rowCount = LocationTravelTimesRowStart + 10;
        const int LocationTravelTimesColStart = 6;
        int colCount = LocationTravelTimesColStart + 10;
        for (int row = LocationTravelTimesRowStart; row <= rowCount; row++)
        {
            var aId = worksheet.Cells[row, LocationTravelTimesColStart].Text;
            if (!int.TryParse(aId, CultureInfo.CurrentCulture, out var locationAId))
            {
                continue;
            }
            for (int col = LocationTravelTimesColStart; col <= colCount; col++)
            {
                var travelTime = worksheet.Cells[row, col].Text;
                if (!int.TryParse(travelTime, NumberStyles.AllowThousands, CultureInfo.GetCultureInfo("de"), out var travelTimeInSeconds))
                {
                    continue;
                }

                var bId = worksheet.Cells[LocationTravelTimesTitle, col].Text;
                if (!int.TryParse(bId, CultureInfo.CurrentCulture, out var locationBId))
                {
                    continue;
                }

                result.Add(new Models.Excel.LocationConnection
                {
                    StartingPointId = locationAId,
                    TravelTimeInSeconds = travelTimeInSeconds,
                    EndPointId = locationBId
                });
            }
        }

        return result;
    }

    public List<Traveler> GetTravelers()
    {
        if (_package is null)
        {
            throw new ArgumentNullException("Package not initiated.");
        }
        var result = new List<Traveler>();
        ExcelWorksheet worksheet = _package.Workbook.Worksheets[0]; // First sheet
        const int LocationRowStart = 16;
        int rowCount = LocationRowStart + 10;
        const int ColId = 1;
        const int ColName = 2;
        const int ColWheelchair = 3;
        for (int row = LocationRowStart; row <= rowCount; row++)
        {

            var id = worksheet.Cells[row, ColId].Text;
            if (!int.TryParse(id, CultureInfo.CurrentCulture, out var travelerId))
            {
                continue;
            }
            var travelerName = worksheet.Cells[row, ColName].Text;

            var wheelchair = worksheet.Cells[row, ColWheelchair].Text;
            var travelerWheelchair = false;
            if (wheelchair == "Ja")
            {
                travelerWheelchair = true;
            }

            result.Add(new Traveler
            {
                Id = travelerId,
                Name = travelerName,
                Wheelchair = travelerWheelchair
            });
        }

        return result;
    }

    public List<Vehicle> GetVehicles()
    {
        if (_package is null)
        {
            throw new ArgumentNullException("Package not initiated.");
        }
        var result = new List<Vehicle>();
        ExcelWorksheet worksheet = _package.Workbook.Worksheets[0]; // First sheet
        const int LocationRowStart = 29;
        int rowCount = LocationRowStart + 3;
        const int ColId = 1;
        const int ColName = 2;
        const int ColPersons = 3;
        const int ColWheelchairs = 4;
        for (int row = LocationRowStart; row <= rowCount; row++)
        {

            var id = worksheet.Cells[row, ColId].Text;
            if (!int.TryParse(id, CultureInfo.CurrentCulture, out var vehicleId))
            {
                continue;
            }
            var vehicleName = worksheet.Cells[row, ColName].Text;

            var persons = worksheet.Cells[row, ColPersons].Text;
            if (!int.TryParse(persons, CultureInfo.CurrentCulture, out var vehiclePersons))
            {
                continue;
            }

            var wheelchairs = worksheet.Cells[row, ColWheelchairs].Text;
            if (!int.TryParse(wheelchairs, CultureInfo.CurrentCulture, out var vehicleWheelchairs))
            {
                continue;
            }

            result.Add(new Vehicle
            {
                Id = vehicleId,
                Name = vehicleName,
                Persons = vehiclePersons,
                Wheelchairs = vehicleWheelchairs
            });
        }

        return result;
    }
}