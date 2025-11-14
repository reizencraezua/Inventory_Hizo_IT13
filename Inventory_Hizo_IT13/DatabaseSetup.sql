-- Inventory Management System Database Setup Script
-- Database: DB_Inventory_Hizo_IT13

-- -- Create Database
-- IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DB_Inventory_Hizo_IT13')
-- BEGIN
--     CREATE DATABASE DB_Inventory_Hizo_IT13;
-- END
-- GO

USE DB_Inventory_Hizo_IT13;
GO

-- Drop tables if they exist (in reverse order of dependencies)
IF OBJECT_ID('tbl_StockTransactions', 'U') IS NOT NULL
    DROP TABLE tbl_StockTransactions;
GO

IF OBJECT_ID('tbl_Products', 'U') IS NOT NULL
    DROP TABLE tbl_Products;
GO

IF OBJECT_ID('tbl_Suppliers', 'U') IS NOT NULL
    DROP TABLE tbl_Suppliers;
GO

IF OBJECT_ID('tbl_Users', 'U') IS NOT NULL
    DROP TABLE tbl_Users;
GO

-- Create tbl_Users table
CREATE TABLE tbl_Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    FullName NVARCHAR(100) NULL,
    UserRole NVARCHAR(20) DEFAULT 'User',
    IsActive BIT DEFAULT 1,
    DateCreated DATETIME DEFAULT GETDATE()
);
GO

-- Create tbl_Suppliers table
CREATE TABLE tbl_Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName NVARCHAR(100) NOT NULL,
    ContactPerson NVARCHAR(100) NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(255) NULL,
    DateAdded DATETIME DEFAULT GETDATE(),
    IsArchived BIT DEFAULT 0
);
GO

-- Create tbl_Products table
CREATE TABLE tbl_Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    ProductDescription NVARCHAR(255) NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    CurrentStock INT DEFAULT 0,
    MinimumStock INT DEFAULT 10,
    CategoryName NVARCHAR(50) NULL,
    DateAdded DATETIME DEFAULT GETDATE(),
    IsArchived BIT DEFAULT 0
);
GO

-- Create tbl_StockTransactions table
CREATE TABLE tbl_StockTransactions (
    TransactionID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    SupplierID INT NULL,
    TransactionType NVARCHAR(20) NOT NULL,
    Quantity INT NOT NULL,
    TransactionDate DATETIME DEFAULT GETDATE(),
    Notes NVARCHAR(255) NULL,
    UserName NVARCHAR(50) NULL,
    FOREIGN KEY (ProductID) REFERENCES tbl_Products(ProductID),
    FOREIGN KEY (SupplierID) REFERENCES tbl_Suppliers(SupplierID)
);
GO

-- Insert default admin user
INSERT INTO tbl_Users (Username, Password, FullName, UserRole, IsActive)
VALUES ('admin', 'admin123', 'Administrator', 'Admin', 1);
GO

-- Insert sample suppliers
INSERT INTO tbl_Suppliers (SupplierName, ContactPerson, PhoneNumber, Email, Address)
VALUES 
    ('Tech Supplies Co.', 'John Smith', '555-0101', 'john@techsupplies.com', '123 Tech Street, City, State 12345'),
    ('Office Depot Inc.', 'Jane Doe', '555-0102', 'jane@officedepot.com', '456 Office Ave, City, State 12346');
GO

-- Insert sample products
INSERT INTO tbl_Products (ProductName, ProductDescription, UnitPrice, CurrentStock, MinimumStock, CategoryName)
VALUES 
    ('Laptop', 'High-performance laptop computer', 999.99, 15, 5, 'Electronics'),
    ('Mouse', 'Wireless optical mouse', 29.99, 8, 10, 'Accessories'),
    ('Keyboard', 'Mechanical gaming keyboard', 79.99, 12, 5, 'Accessories');
GO

PRINT 'Database setup completed successfully!';
PRINT 'Default admin user: admin / admin123';
GO

