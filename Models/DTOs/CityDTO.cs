namespace TodoApi.Models.DTOs
{
    public class CityDTO
    {
        public int Id { get;  set; }
        public string Name { get;  set; }
        public string CountryCode { get;  set; }
        public int Population { get;  set; }

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

         public void SetId(int id)
        {
            Id = id;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetCountryCode(string countryCode)
        {
            CountryCode = countryCode;
        }

        public void SetPopulation(int population)
        {
            Population = population;
        }

        
    }
}
