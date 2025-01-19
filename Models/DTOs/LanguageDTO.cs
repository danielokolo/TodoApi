
namespace TodoApi.Models.DTOs;

public class LanguageDTO
{
    public string CountryCode { get; set; }
    public string Language { get; set; }
    public bool IsOfficial { get; set; }
    public float Percentage { get; set; }
}