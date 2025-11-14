using Inventory_Hizo_IT13.Models;
using Microsoft.Data.SqlClient;

namespace Inventory_Hizo_IT13.Services;

public class StockService
{
    private readonly DatabaseService _databaseService;

    public StockService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<bool> AddStockAsync(int productId, int supplierId, int quantity, string? notes, string? userName)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var insertQuery = @"
                    INSERT INTO tbl_StockTransactions (ProductID, SupplierID, TransactionType, Quantity, Notes, UserName)
                    VALUES (@ProductID, @SupplierID, 'ADD', @Quantity, @Notes, @UserName)";

                using var insertCommand = new SqlCommand(insertQuery, connection, transaction);
                insertCommand.Parameters.AddWithValue("@ProductID", productId);
                insertCommand.Parameters.AddWithValue("@SupplierID", supplierId);
                insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                insertCommand.Parameters.AddWithValue("@Notes", (object?)notes ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@UserName", (object?)userName ?? DBNull.Value);
                await insertCommand.ExecuteNonQueryAsync();

                var updateQuery = @"
                    UPDATE tbl_Products
                    SET CurrentStock = CurrentStock + @Quantity
                    WHERE ProductID = @ProductID";

                using var updateCommand = new SqlCommand(updateQuery, connection, transaction);
                updateCommand.Parameters.AddWithValue("@ProductID", productId);
                updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                await updateCommand.ExecuteNonQueryAsync();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveStockAsync(int productId, int quantity, string? notes, string? userName)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            
            var checkQuery = "SELECT CurrentStock FROM tbl_Products WHERE ProductID = @ProductID";
            using var checkCommand = new SqlCommand(checkQuery, connection);
            checkCommand.Parameters.AddWithValue("@ProductID", productId);
            var currentStock = (int)await checkCommand.ExecuteScalarAsync();

            if (currentStock < quantity)
            {
                return false;
            }

            using var transaction = connection.BeginTransaction();

            try
            {
                var insertQuery = @"
                    INSERT INTO tbl_StockTransactions (ProductID, SupplierID, TransactionType, Quantity, Notes, UserName)
                    VALUES (@ProductID, NULL, 'REMOVE', @Quantity, @Notes, @UserName)";

                using var insertCommand = new SqlCommand(insertQuery, connection, transaction);
                insertCommand.Parameters.AddWithValue("@ProductID", productId);
                insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                insertCommand.Parameters.AddWithValue("@Notes", (object?)notes ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@UserName", (object?)userName ?? DBNull.Value);
                await insertCommand.ExecuteNonQueryAsync();

                var updateQuery = @"
                    UPDATE tbl_Products
                    SET CurrentStock = CurrentStock - @Quantity
                    WHERE ProductID = @ProductID";

                using var updateCommand = new SqlCommand(updateQuery, connection, transaction);
                updateCommand.Parameters.AddWithValue("@ProductID", productId);
                updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                await updateCommand.ExecuteNonQueryAsync();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<StockTransaction>> GetTransactionHistoryAsync()
    {
        var transactions = new List<StockTransaction>();
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = @"
                SELECT 
                    t.TransactionID,
                    t.ProductID,
                    t.SupplierID,
                    t.TransactionType,
                    t.Quantity,
                    t.TransactionDate,
                    t.Notes,
                    t.UserName,
                    p.ProductName,
                    s.SupplierName
                FROM tbl_StockTransactions t
                LEFT JOIN tbl_Products p ON t.ProductID = p.ProductID
                LEFT JOIN tbl_Suppliers s ON t.SupplierID = s.SupplierID
                ORDER BY t.TransactionDate DESC";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                transactions.Add(new StockTransaction
                {
                    TransactionID = reader.GetInt32(0),
                    ProductID = reader.GetInt32(1),
                    SupplierID = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                    TransactionType = reader.GetString(3),
                    Quantity = reader.GetInt32(4),
                    TransactionDate = reader.GetDateTime(5),
                    Notes = reader.IsDBNull(6) ? null : reader.GetString(6),
                    UserName = reader.IsDBNull(7) ? null : reader.GetString(7),
                    ProductName = reader.IsDBNull(8) ? null : reader.GetString(8),
                    SupplierName = reader.IsDBNull(9) ? null : reader.GetString(9)
                });
            }
        }
        catch
        {
        }
        return transactions;
    }
}

