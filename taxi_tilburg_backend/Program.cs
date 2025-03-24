using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taxi_tilburg_backend.Database;
using taxi_tilburg_backend.Database.Models;
using taxi_tilburg_backend.Models.Requests;
using taxi_tilburg_backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaxiTilburgContext>(opt => opt.UseInMemoryDatabase("TaxiTilburg"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationConnectionRepository, LocationConnectionRepository>();
builder.Services.AddScoped<IExcelImporter, EPPlusExcelImporter>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TaxiTilburgAPI";
    config.Title = "TaxiTilburgAPI v1";
    config.Version = "v1";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5102") // Add trusted domains
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowedOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TaxiTilburgAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/database/locations", async (ILocationService locationService) =>
{
    return await locationService.ListLocations();
})
    .WithTags("Database");

app.MapGet("/database/location/{id}", async (int id, ILocationService locationService) =>
{
    return await locationService.GetLocationAsync(id);
})
    .WithTags("Database");

app.MapPost("/upload", async (
    [FromServices] IExcelImporter excelImporter,
    ILocationService locationService,
    IRepository<Location> locationRepo,
    IRepository<Traveler> travelerRepo,
    IRepository<Vehicle> vehicleRepo,
    IFormFile file) =>
{
    if (file == null || file.Length == 0)
    {
        throw new ArgumentException("Invalid file.");
    }

    using var stream = new MemoryStream();
    await file.CopyToAsync(stream);
    using var importer = excelImporter.FromStream(stream);

    var locations = importer.GetLocations();
    locationService.AddLocation(locations);
    var locationDistances = importer.GetLocationDistances();
    await locationService.AddConnectionsAsync(locationDistances);
    var locationTravelTimes = importer.GetLocationTravelTimes();
    await locationService.AddConnectionsAsync(locationTravelTimes);

    var travelers = importer.GetTravelers();
    travelers.ForEach(async t =>
    {
        if (await travelerRepo.GetByIdAsync(t.Id) is null)
        {
            await travelerRepo.AddAsync(t);
        }
    });
    var vehicles = importer.GetVehicles();
    vehicles.ForEach(async v =>
    {
        if (await vehicleRepo.GetByIdAsync(v.Id) is null)
        {
            await vehicleRepo.AddAsync(v);
        }
    });
})
.WithTags("Excel import")
.RequireCors("AllowedOrigins")
.DisableAntiforgery(); // todo

void RegisterExcelParsingEndpoint<T>(WebApplication app, string route, Func<IExcelImporter, IFormFile, List<T>> handler)
{
    app.MapPost(route, async ([FromServices] IExcelImporter excelImporter, IFormFile file) =>
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Invalid file.");
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        using var importer = excelImporter.FromStream(stream);
        return handler(excelImporter, file);
    })
    .WithTags("Excel import")
    .RequireCors("AllowedOrigins")
    .DisableAntiforgery(); // todo
}
RegisterExcelParsingEndpoint(app, "get-locations", ([FromServices] excelImporter, file) =>
{
    return excelImporter.GetLocations();
});
RegisterExcelParsingEndpoint(app, "get-travelers", ([FromServices] excelImporter, file) =>
{
    return excelImporter.GetTravelers();
});
RegisterExcelParsingEndpoint(app, "get-vehicles", ([FromServices] excelImporter, file) =>
{
    return excelImporter.GetVehicles();
});
RegisterExcelParsingEndpoint(app, "get-location-distances", ([FromServices] excelImporter, file) =>
{
    return excelImporter.GetLocationDistances();
});
RegisterExcelParsingEndpoint(app, "get-location-traveltimes", ([FromServices] excelImporter, file) =>
{
    return excelImporter.GetLocationTravelTimes();
});

app.MapPost("/logic/calculate-route", async (RouteRequest req, ILocationService locationService) =>
{
    return await locationService.CalculateRouteAsync(req);
})
    .WithTags("Logic");

app.Run();
