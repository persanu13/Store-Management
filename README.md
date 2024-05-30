# Project Description: Store Management System

## Technologies Used
- **C#** for application logic.
- **WPF (Windows Presentation Foundation)** for the graphical user interface.
- **SQL Server** for database management.

## Project Overview
The project is a desktop application designed to manage a store, allowing two types of users: Admin and Cashier. The Admin has full access to the database, including viewing, adding, modifying, and deleting records. The Cashier can create and view receipts.

## Features

### Authentication
- Users can log into the application through a login screen.
- After authentication, users are redirected to the appropriate dashboard based on their role (Admin or Cashier).

### Admin Dashboard
- **View Tables**: The Admin can view all tables in the database, such as Products, Customers, Sales, and Users.
- **CRUD Operations**: The Admin can add, modify, and delete records in the tables.
- **Reporting**: The Admin can generate reports and statistics.

### Cashier Dashboard
- **Create Receipts**: The Cashier can create new receipts for sales transactions.
- **View Receipts**: The Cashier can view all receipts they have created.

## User Interface

### Login Window
- Fields for username and password.
- Login button.

### Admin Dashboard
- Navigation menu to access different tables (Products, Customers, Sales, Users).
- Data grids to display table contents.
- Forms to add, edit, and delete records.
- Buttons to generate reports.

### Cashier Dashboard
- Form to create new receipts with fields for customer selection, product selection, quantity, and price.
- Data grid to display created receipts.

## Implementation Details

- **WPF**: Used for creating the user interface with XAML for designing windows and controls.
- **Entity Framework**: Used for database operations, providing an ORM to interact with SQL Server.
- **MVVM Pattern**: Used to separate the business logic from the user interface, making the code more maintainable.

## Workflow

### Login Process
- User enters username and password.
- System checks credentials against the Users table.
- If valid, the user is redirected to the appropriate dashboard based on their role.

### Admin Operations
- Admin can navigate through different sections using the navigation menu.
- Each section displays a data grid with records from the respective table.
- Admin can add, edit, or delete records through forms that appear in modal windows.

### Cashier Operations
- Cashier can create new receipts by selecting a customer, adding products, and specifying quantities.
- The total amount is calculated automatically.
- Receipts are saved in the Sales and SaleDetails tables.

This project aims to provide a robust and user-friendly system for managing store operations, with distinct functionalities tailored for Admins and Cashiers.
