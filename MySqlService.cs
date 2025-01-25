using MySql.Data.MySqlClient; // Necesario para trabajar con MySQL



public class MySqlService<T> where T : class, IEntity, new()
{
    private string _connectionString;

    public MySqlService(string connectionString)
    {
        _connectionString = connectionString;
    }

    private MySqlConnection getConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    // Método para obtener el nombre del campo clave primaria (puede ser "Id" o "Code")
    private string GetPrimaryKeyName()
    {
        var keyProperty = typeof(T).GetProperties().FirstOrDefault(p =>
            p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) || 
            p.Name.Equals("Code", StringComparison.OrdinalIgnoreCase));
        return keyProperty?.Name;
    }

    // Método para obtener el tipo de la clave primaria (int, string, etc.)
    private Type GetPrimaryKeyType()
    {
        var keyProperty = typeof(T).GetProperties().FirstOrDefault(p =>
            p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
            p.Name.Equals("Code", StringComparison.OrdinalIgnoreCase));
        return keyProperty?.PropertyType;
    }

    // Método para obtener el valor de la clave primaria de una entidad
    private object GetPrimaryKeyValue(T entity)
    {
        var keyProperty = typeof(T).GetProperties().FirstOrDefault(p =>
            p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
            p.Name.Equals("Code", StringComparison.OrdinalIgnoreCase));
        return keyProperty?.GetValue(entity);
    }

    // Get all records
    public List<T> GetAll(string tableName)
    {
        List<T> entities = new List<T>();
        using (var connection = getConnection())
        {
            connection.Open();
            string query = $"SELECT * FROM {tableName}";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                T entity = MapReaderToEntity(reader);
                entities.Add(entity);
            }
        }
        return entities;
    }

    // Get record by primary key
    public T GetByPrimaryKey(string tableName, object keyValue)
    {
        T entity = null;
        using (var connection = getConnection())
        {
            connection.Open();
            string primaryKey = GetPrimaryKeyName();  // Obtener nombre de la clave primaria
            string query = $"SELECT * FROM {tableName} WHERE {primaryKey} = @keyValue";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@keyValue", keyValue);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                entity = MapReaderToEntity(reader);
            }
        }
        return entity;
    }

    // Add a new record
    public T Add(string tableName, T entity)
    {
        using (var connection = getConnection())
        {
            connection.Open();
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id" && p.Name != "Code").ToList();
            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            string query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameters})";
            MySqlCommand command = new MySqlCommand(query, connection);

            foreach (var property in properties)
            {
                command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity));
            }

            command.ExecuteNonQuery();
            // Después de la inserción, asignar el Id o Code (dependiendo de la entidad)
            if (GetPrimaryKeyType() == typeof(int))
            {
                entity.GetType().GetProperty("Id")?.SetValue(entity, (int)command.LastInsertedId);
            }
        }
        return entity;
    }

    // Update an existing record
    public T Update(string tableName, T entity)
    {
        using (var connection = getConnection())
        {
            connection.Open();
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id" && p.Name != "Code").ToList();
            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            string primaryKey = GetPrimaryKeyName();
            string query = $"UPDATE {tableName} SET {setClause} WHERE {primaryKey} = @keyValue";
            MySqlCommand command = new MySqlCommand(query, connection);

            foreach (var property in properties)
            {
                command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity));
            }
            command.Parameters.AddWithValue("@keyValue", GetPrimaryKeyValue(entity));

            command.ExecuteNonQuery();
        }
        return entity;
    }

    // Delete a record by primary key
    public void Delete(string tableName, object keyValue)
    {
        using (var connection = getConnection())
        {
            connection.Open();
            string primaryKey = GetPrimaryKeyName();
            string query = $"DELETE FROM {tableName} WHERE {primaryKey} = @keyValue";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@keyValue", keyValue);
            command.ExecuteNonQuery();
        }
    }

    // Helper method to map a data reader to an entity
    private T MapReaderToEntity(MySqlDataReader reader)
    {
        T entity = new T();
        foreach (var property in typeof(T).GetProperties())
        {
            if (reader.HasColumn(property.Name) && !reader.IsDBNull(reader.GetOrdinal(property.Name)))
            {
                property.SetValue(entity, reader.GetValue(reader.GetOrdinal(property.Name)));
            }
        }
        return entity;
    }
}

// Extension method to check if a column exists in a data reader
public static class MySqlDataReaderExtensions
{
    public static bool HasColumn(this MySqlDataReader reader, string columnName)
    {
        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}
