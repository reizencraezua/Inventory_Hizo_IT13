namespace Inventory_Hizo_IT13.Models;

public class StockTransaction
{
    public int TransactionID { get; set; }
    public int ProductID { get; set; }
    public int? SupplierID { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Notes { get; set; }
    public string? UserName { get; set; }
    
    public string? ProductName { get; set; }
    public string? SupplierName { get; set; }
}

