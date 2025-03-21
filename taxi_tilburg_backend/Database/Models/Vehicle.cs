namespace taxi_tilburg_backend.Database.Models;

public class Vehicle
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Persons { get; set; }
    public int Wheelchairs { get; set; }
}