namespace TodoApi.Models.DTOs
{
    public class CityDTO
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string CountryCode { get; private set; }
        public int Population { get; private set; }

        // Constructor
        public CityDTO(int id, string name, string countryCode, int population)
        {
            Id = id;
            Name = name;
            CountryCode = countryCode;
            Population = population;
        }

        // Getters (estos son implícitos al usar { get; private set; })
        public int GetId()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetCountryCode()
        {
            return CountryCode;
        }

        public int GetPopulation()
        {
            return Population;
        }
    }
}
