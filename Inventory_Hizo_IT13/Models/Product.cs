namespace Inventory_Hizo_IT13.Models;

public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductDescription { get; set; }
    public decimal UnitPrice { get; set; }
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    public string? CategoryName { get; set; }
    public DateTime DateAdded { get; set; }
    public bool IsArchived { get; set; }
}

