using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using taxi_tilburg_backend.Database;
using taxi_tilburg_backend.Database.Models;
using taxi_tilburg_backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaxiTilburgContext>(opt => opt.UseInMemoryDatabase("TaxiTilburg"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationDistanceRepository, LocationDistanceRepository>();
builder.Services.AddScoped<ILocationTravelTimeRepository, LocationTravelTimeRepository>();
builder.Services.AddScoped<IExcelImporter, EPPlusExcelImporter>();
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

app.MapGet("/location/list", async (ILocationRepository repo) =>
    await repo.GetAllAsync())
    .WithTags("Database");

app.MapGet("/location/{id}", async (int id, ILocationRepository repo) =>
{
    return await repo.GetByIdAsync(id);
})
    .WithTags("Database");

app.MapPost("/locationitems", async (Location todo, TaxiTilburgContext db) =>
{
    db.Locations.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/locationitems/{todo.Id}", todo);
})
.WithTags("Database");

app.MapPut("/locationitems/{id}", async (int id, Location inputTodo, ILocationRepository repo) =>
{
    var todo = await repo.GetByIdAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.Latitude = inputTodo.Latitude;
    todo.Longitude = inputTodo.Longitude;

    await repo.UpdateAsync(todo);

    return Results.NoContent();
})
.WithTags("Database");

app.MapDelete("/locationitems/{id}", async (int id, ILocationRepository repo) =>
{
    if (await repo.GetByIdAsync(id) is Location todo)
    {
        await repo.DeleteAsync(id);
        return Results.NoContent();
    }

    return Results.NotFound();
})
.WithTags("Database");

app.MapPost("/upload", async (
    [FromServices] IExcelImporter excelImporter,
    IRepository<Location> locationRepo,
    IRepository<Traveler> travelerRepo,
    IRepository<Vehicle> vehicleRepo,
    ILocationDistanceRepository distanceRepo,
    ILocationTravelTimeRepository travelTimeRepo,
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
    locations.ForEach(async l =>
    {
        if (await locationRepo.GetByIdAsync(l.Id) is null)
        {
            await locationRepo.AddAsync(l);
        }
    });
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
    var locationDistances = importer.GetLocationDistances();
    locationDistances.ForEach(async ld =>
    {
        if (await distanceRepo.GetByIdAsync(ld.StartingPointId, ld.EndPointId) is null)
        {
            await distanceRepo.AddAsync(ld);
        }
    });
    var locationTravelTimes = importer.GetLocationTravelTimes();
    locationTravelTimes.ForEach(async lt =>
    {
        if (await travelTimeRepo.GetByIdAsync(lt.StartingPointId, lt.EndPointId) is null)
        {
            await travelTimeRepo.AddAsync(lt);
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

app.MapGet("/", () => "Hello World!");

app.Run();
