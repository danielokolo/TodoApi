namespace TodoApi.Models.DTOs;

public class CountryDTO
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Continent { get; set; }
    public string Region { get; set; }
    public float SurfaceArea { get; set; }
    public int Population { get; set; }
}