using Microsoft.Data.SqlClient;

namespace Inventory_Hizo_IT13.Services;

public class DatabaseService
{
    private readonly string _connectionString = 
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_Inventory_Hizo_IT13;Integrated Security=True;TrustServerCertificate=True";

    public async Task<SqlConnection> GetConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var connection = await GetConnectionAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

