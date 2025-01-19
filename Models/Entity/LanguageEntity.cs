
namespace TodoApi.Models.Entity;

public class LanguageEntity
{
    public string CountryCode { get; set; }
    public string LanguageCountry { get; set; }
    public bool IsOfficial { get; set; }
    public float Percentage { get; set; }
}