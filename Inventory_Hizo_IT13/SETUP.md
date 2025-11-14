# Inventory Management System - Setup Guide

## Prerequisites

1. **.NET 9.0 SDK** - Ensure you have .NET 9.0 SDK installed
2. **SQL Server LocalDB** - Required for the database
3. **Visual Studio 2022** or **Visual Studio Code** with C# extension

## Database Setup

1. Open SQL Server Management Studio (SSMS) or use `sqlcmd`
2. Execute the `DatabaseSetup.sql` script located in the project root
   - This will create the database `DB_Inventory_Hizo_IT13`
   - Create all required tables
   - Insert sample data including default admin user

3. **Default Login Credentials:**
   - Username: `admin`
   - Password: `admin123`

## Running the Application

1. **Restore NuGet Packages:**
   ```bash
   dotnet restore
   ```

2. **Build the Project:**
   ```bash
   dotnet build
   ```

3. **Run the Application:**
   ```bash
   dotnet run
   ```

   Or use your IDE's run button (F5 in Visual Studio)

## Project Structure

```
Inventory_Hizo_IT13/
├── Components/
│   ├── Layout/
│   │   ├── LoginLayout.razor       # Login page layout
│   │   ├── MainLayout.razor        # Main application layout
│   │   └── NavMenu.razor           # Navigation menu
│   ├── Pages/
│   │   ├── Login.razor             # Login page
│   │   ├── Home.razor              # Dashboard
│   │   ├── Products.razor          # Product management
│   │   ├── Stock.razor             # Stock transactions
│   │   ├── Suppliers.razor         # Supplier management
│   │   └── Transactions.razor      # Transaction history
│   └── RedirectToLogin.razor      # Route protection component
├── Models/
│   ├── Product.cs
│   ├── Supplier.cs
│   ├── StockTransaction.cs
│   └── User.cs
├── Services/
│   ├── DatabaseService.cs         # Database connection management
│   ├── AuthService.cs              # Authentication service
│   ├── ProductService.cs           # Product CRUD operations
│   ├── SupplierService.cs         # Supplier CRUD operations
│   └── StockService.cs             # Stock transaction operations
├── DatabaseSetup.sql               # Database initialization script
└── MauiProgram.cs                  # Application startup & DI configuration
```

## Key Features

- ✅ User Authentication (Login/Logout)
- ✅ Dashboard with Statistics
- ✅ Product Management (CRUD)
- ✅ Supplier Management (CRUD)
- ✅ Stock Transactions (Add/Remove)
- ✅ Transaction History with Filtering
- ✅ Low Stock Alerts
- ✅ Search Functionality
- ✅ Responsive Design

## Database Connection

The connection string is configured in `Services/DatabaseService.cs`:
```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DB_Inventory_Hizo_IT13;Integrated Security=True;TrustServerCertificate=True
```

To change the database location, modify the `_connectionString` field in `DatabaseService.cs`.

## Troubleshooting

### Database Connection Issues
- Ensure SQL Server LocalDB is installed and running
- Verify the database `DB_Inventory_Hizo_IT13` exists
- Check that the connection string matches your SQL Server instance

### Build Errors
- Run `dotnet restore` to restore NuGet packages
- Ensure you have .NET 9.0 SDK installed
- Check that `Microsoft.Data.SqlClient` package is restored

### Runtime Errors
- Verify the database is set up correctly
- Check that all services are registered in `MauiProgram.cs`
- Ensure the database contains the required tables

## Next Steps

1. Execute `DatabaseSetup.sql` to create the database
2. Build and run the application
3. Login with default credentials (admin/admin123)
4. Start managing your inventory!

## Notes

- Passwords are stored in plain text (for demonstration purposes only)
- For production use, implement proper password hashing
- Consider implementing additional security measures
- The system uses soft deletes (archiving) for data integrity

