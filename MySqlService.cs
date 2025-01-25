using MySql.Data.MySqlClient;
using TodoApi.Models.DTOs;
using TodoApi.Models.Entity;
using System.Collections.Generic;

public class MySqlService
{
    private string _connectionString;

    public MySqlService(string connectionString)
    {
        this._connectionString = connectionString;
    }

    // Get a new MySql connection
    public MySqlConnection getConnection()
    {
        return new MySqlConnection(this._connectionString);
    }

    // Get all cities
    public List<CityDTO> getAllCities()
    {
        List<CityDTO> cities = new List<CityDTO>();
        using (var connection = getConnection())
        {
            connection.Open();
            string query = "SELECT * FROM city";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                cities.Add(new CityDTO(
                    reader.GetInt32("ID"),
                    reader.GetString("Name"),
                    reader.GetString("CountryCode"),
                    reader.GetInt32("Population")
                ));
            }
        }
        return cities;
    }

    // Get city by ID
    public CityDTO GetCityById(int id)
    {
        CityDTO city = null;
        using (var connection = getConnection())
        {
            connection.Open();
            string query = "SELECT * FROM city WHERE ID = @id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                city = new CityDTO(
                    reader.GetInt32("ID"),
                    reader.GetString("Name"),
                    reader.GetString("CountryCode"),
                    reader.GetInt32("Population")
                );
            }
        }
        return city;
    }

    // Add a new city
    public City AddCity(City city)
    {
        using (var connection = getConnection())
        {
            connection.Open();
            string query = "INSERT INTO city (Name, CountryCode, Population) VALUES (@name, @countryCode, @population)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", city.Name);
            command.Parameters.AddWithValue("@countryCode", city.CountryCode);
            command.Parameters.AddWithValue("@population", city.Population);
            command.ExecuteNonQuery();

            // Set the ID of the inserted city (assumes AUTO_INCREMENT in MySQL)
            city.Id = (int)command.LastInsertedId;
        }
        return city;
    }

    // Update an existing city
    public City UpdateCity(City city)
    {
        using (var connection = getConnection())
        {
            connection.Open();
            string query = "UPDATE city SET Name = @name, CountryCode = @countryCode, Population = @population WHERE ID = @id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", city.Name);
            command.Parameters.AddWithValue("@countryCode", city.CountryCode);
            command.Parameters.AddWithValue("@population", city.Population);
            command.Parameters.AddWithValue("@id", city.Id);
            command.ExecuteNonQuery();
        }
        return city;
    }

    // Delete a city by ID
    public void DeleteCity(int id)
    {
        using (var connection = getConnection())
        {
            connection.Open();
            string query = "DELETE FROM city WHERE ID = @id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
