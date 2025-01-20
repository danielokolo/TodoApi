using MySql.Data.MySqlClient;
using TodoApi.Models.DTOs;

public class MySqlService
{
    string _connectionString;

    public MySqlService( string _connectionString){
            this._connectionString = _connectionString;
    }

    public MySqlConnection getConnection(){
        return new MySqlConnection(this._connectionString);
    }

    public List<CityDTO> getAllCitys()
 {
     List<CityDTO> citys = new List<CityDTO>();
     using (var connection = getConnection())
     {
         connection.Open();
         string query = "SELECT * FROM city";
         MySqlCommand command = new MySqlCommand(query, connection);
         MySqlDataReader reader = command.ExecuteReader();
         while (reader.Read())
         {
         citys.Add(new CityDTO(
    reader.GetInt32("ID"),
    reader.GetString("Name"),
    reader.GetString("CountryCode"),
    reader.GetInt32("Population")
         )
);

     }
     }
     return citys;
 }
}