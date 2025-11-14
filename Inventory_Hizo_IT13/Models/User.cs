namespace Inventory_Hizo_IT13.Models;

public class User
{
    public int UserID { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string UserRole { get; set; } = "User";
    public bool IsActive { get; set; } = true;
    public DateTime DateCreated { get; set; }
}

