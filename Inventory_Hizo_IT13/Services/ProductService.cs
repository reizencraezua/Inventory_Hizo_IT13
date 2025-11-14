using Inventory_Hizo_IT13.Models;
using Microsoft.Data.SqlClient;

namespace Inventory_Hizo_IT13.Services;

public class ProductService
{
    private readonly DatabaseService _databaseService;

    public ProductService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<Product>> GetAllProductsAsync(bool showOnlyArchived = false)
    {
        var products = new List<Product>();
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = showOnlyArchived
                ? "SELECT ProductID, ProductName, ProductDescription, UnitPrice, CurrentStock, MinimumStock, CategoryName, DateAdded, IsArchived FROM tbl_Products WHERE IsArchived = 1"
                : "SELECT ProductID, ProductName, ProductDescription, UnitPrice, CurrentStock, MinimumStock, CategoryName, DateAdded, IsArchived FROM tbl_Products WHERE IsArchived = 0";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    ProductID = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    ProductDescription = reader.IsDBNull(2) ? null : reader.GetString(2),
                    UnitPrice = reader.GetDecimal(3),
                    CurrentStock = reader.GetInt32(4),
                    MinimumStock = reader.GetInt32(5),
                    CategoryName = reader.IsDBNull(6) ? null : reader.GetString(6),
                    DateAdded = reader.GetDateTime(7),
                    IsArchived = reader.GetBoolean(8)
                });
            }
        }
        catch
        {
        }
        return products;
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = @"
                INSERT INTO tbl_Products (ProductName, ProductDescription, UnitPrice, CurrentStock, MinimumStock, CategoryName)
                VALUES (@ProductName, @ProductDescription, @UnitPrice, @CurrentStock, @MinimumStock, @CategoryName)";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@ProductDescription", (object?)product.ProductDescription ?? DBNull.Value);
            command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
            command.Parameters.AddWithValue("@CurrentStock", product.CurrentStock);
            command.Parameters.AddWithValue("@MinimumStock", product.MinimumStock);
            command.Parameters.AddWithValue("@CategoryName", (object?)product.CategoryName ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = @"
                UPDATE tbl_Products
                SET ProductName = @ProductName,
                    ProductDescription = @ProductDescription,
                    UnitPrice = @UnitPrice,
                    MinimumStock = @MinimumStock,
                    CategoryName = @CategoryName
                WHERE ProductID = @ProductID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", product.ProductID);
            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@ProductDescription", (object?)product.ProductDescription ?? DBNull.Value);
            command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
            command.Parameters.AddWithValue("@MinimumStock", product.MinimumStock);
            command.Parameters.AddWithValue("@CategoryName", (object?)product.CategoryName ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ArchiveProductAsync(int productId)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = "UPDATE tbl_Products SET IsArchived = 1 WHERE ProductID = @ProductID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", productId);

            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RestoreProductAsync(int productId)
    {
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = "UPDATE tbl_Products SET IsArchived = 0 WHERE ProductID = @ProductID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", productId);

            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<Product>> SearchProductsAsync(string searchTerm)
    {
        var products = new List<Product>();
        try
        {
            using var connection = await _databaseService.GetConnectionAsync();
            var query = @"
                SELECT ProductID, ProductName, ProductDescription, UnitPrice, CurrentStock, MinimumStock, CategoryName, DateAdded, IsArchived
                FROM tbl_Products
                WHERE IsArchived = 0 AND (
                    ProductName LIKE @SearchTerm OR
                    ProductDescription LIKE @SearchTerm OR
                    CategoryName LIKE @SearchTerm
                )";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    ProductID = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    ProductDescription = reader.IsDBNull(2) ? null : reader.GetString(2),
                    UnitPrice = reader.GetDecimal(3),
                    CurrentStock = reader.GetInt32(4),
                    MinimumStock = reader.GetInt32(5),
                    CategoryName = reader.IsDBNull(6) ? null : reader.GetString(6),
                    DateAdded = reader.GetDateTime(7),
                    IsArchived = reader.GetBoolean(8)
                });
            }
        }
        catch
        {
        }
        return products;
    }
}

