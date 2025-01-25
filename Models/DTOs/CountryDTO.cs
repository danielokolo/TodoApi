namespace TodoApi.Models.DTOs;

public class CountryDTO : IEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Continent { get; set; }
    public string Region { get; set; }
    public decimal SurfaceArea { get; set; }
    public int Population { get; set; }
}