
namespace TodoApi.Models.DTOs;

public class LanguageDTO :  IEntity
{
    public string CountryCode { get; set; }
    public string LanguageCountry { get; set; }
    public string IsOfficial { get; set; }
    public decimal Percentage { get; set; }
}