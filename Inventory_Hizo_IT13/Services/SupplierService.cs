using Inventory_Hizo_IT13.Models;
using Microsoft.Data.SqlClient;

namespace Inventory_Hizo_IT13.Services;

public class SupplierService
{
    private readonly DatabaseService _databaseService;

    public SupplierService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<Supplier>> GetAllSuppliersAsync(bool showOnlyArchived = false)
    {
        var suppliers = new List<Supplier>();
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = showOnlyArchived
                ? "SELECT SupplierID, SupplierName, ContactPerson, PhoneNumber, Email, Address, DateAdded, IsArchived FROM tbl_Suppliers WHERE IsArchived = 1"
                : "SELECT SupplierID, SupplierName, ContactPerson, PhoneNumber, Email, Address, DateAdded, IsArchived FROM tbl_Suppliers WHERE IsArchived = 0";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                suppliers.Add(new Supplier
                {
                    SupplierID = reader.GetInt32(0),
                    SupplierName = reader.GetString(1),
                    ContactPerson = reader.IsDBNull(2) ? null : reader.GetString(2),
                    PhoneNumber = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                    DateAdded = reader.GetDateTime(6),
                    IsArchived = reader.GetBoolean(7)
                });
            }
        }
        catch
        {
        }
        return suppliers;
    }

    public async Task<bool> AddSupplierAsync(Supplier supplier)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = @"
                INSERT INTO tbl_Suppliers (SupplierName, ContactPerson, PhoneNumber, Email, Address)
                VALUES (@SupplierName, @ContactPerson, @PhoneNumber, @Email, @Address)";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
            command.Parameters.AddWithValue("@ContactPerson", (object?)supplier.ContactPerson ?? DBNull.Value);
            command.Parameters.AddWithValue("@PhoneNumber", (object?)supplier.PhoneNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)supplier.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@Address", (object?)supplier.Address ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateSupplierAsync(Supplier supplier)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = @"
                UPDATE tbl_Suppliers
                SET SupplierName = @SupplierName,
                    ContactPerson = @ContactPerson,
                    PhoneNumber = @PhoneNumber,
                    Email = @Email,
                    Address = @Address
                WHERE SupplierID = @SupplierID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SupplierID", supplier.SupplierID);
            command.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
            command.Parameters.AddWithValue("@ContactPerson", (object?)supplier.ContactPerson ?? DBNull.Value);
            command.Parameters.AddWithValue("@PhoneNumber", (object?)supplier.PhoneNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)supplier.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@Address", (object?)supplier.Address ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ArchiveSupplierAsync(int supplierId)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = "UPDATE tbl_Suppliers SET IsArchived = 1 WHERE SupplierID = @SupplierID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SupplierID", supplierId);

            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RestoreSupplierAsync(int supplierId)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = "UPDATE tbl_Suppliers SET IsArchived = 0 WHERE SupplierID = @SupplierID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SupplierID", supplierId);

            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<Supplier>> SearchSuppliersAsync(string searchTerm)
    {
        var suppliers = new List<Supplier>();
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = @"
                SELECT SupplierID, SupplierName, ContactPerson, PhoneNumber, Email, Address, DateAdded, IsArchived
                FROM tbl_Suppliers
                WHERE IsArchived = 0 AND (
                    SupplierName LIKE @SearchTerm OR
                    ContactPerson LIKE @SearchTerm OR
                    PhoneNumber LIKE @SearchTerm OR
                    Email LIKE @SearchTerm OR
                    Address LIKE @SearchTerm
                )";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                suppliers.Add(new Supplier
                {
                    SupplierID = reader.GetInt32(0),
                    SupplierName = reader.GetString(1),
                    ContactPerson = reader.IsDBNull(2) ? null : reader.GetString(2),
                    PhoneNumber = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                    DateAdded = reader.GetDateTime(6),
                    IsArchived = reader.GetBoolean(7)
                });
            }
        }
        catch
        {
        }
        return suppliers;
    }
}

