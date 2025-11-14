using Inventory_Hizo_IT13.Models;
using Microsoft.Data.SqlClient;

namespace Inventory_Hizo_IT13.Services;

public class AuthService
{
    private readonly DatabaseService _databaseService;
    private User? _currentUser;

    public AuthService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public bool IsAuthenticated => _currentUser != null;
    public User? CurrentUser => _currentUser;

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = @"
                SELECT UserID, Username, Password, FullName, UserRole, IsActive, DateCreated
                FROM tbl_Users
                WHERE Username = @Username AND Password = @Password AND IsActive = 1";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                _currentUser = new User
                {
                    UserID = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    FullName = reader.IsDBNull(3) ? null : reader.GetString(3),
                    UserRole = reader.GetString(4),
                    IsActive = reader.GetBoolean(5),
                    DateCreated = reader.GetDateTime(6)
                };
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public void Logout()
    {
        _currentUser = null;
    }
}

