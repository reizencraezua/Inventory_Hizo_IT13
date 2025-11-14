namespace Inventory_Hizo_IT13.Models;

public class Supplier
{
    public int SupplierID { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime DateAdded { get; set; }
    public bool IsArchived { get; set; }
}

