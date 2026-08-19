# ABC Retail Web App

## Overview

ABC Retail Web App is an ASP.NET Core MVC application that helps manage retail data through Azure Storage services.

The application provides pages for customers, products, order transactions, and file storage.

## Azure Storage Features

- **Azure Table Storage**  
  Stores customer and product records.

- **Azure Blob Storage**  
  Stores product images.

- **Azure Queue Storage**  
  Stores order or transaction messages.

- **Azure File Storage**  
  Stores text files.

## Application Pages

- Home
- Customers
- Products
- Order Queue
- File Storage

## Technologies Used

- ASP.NET Core MVC
- C#
- Azure Storage
- Bootstrap
- Visual Studio

## Running the Application

1. Clone or download this repository.
2. Open the solution in Visual Studio.
3. Configure the Azure Storage connection string in `appsettings.json` or User Secrets.
4. Build the solution.
5. Run the application.

## Important Note

Azure connection strings, storage account keys, and other secrets must not be uploaded to GitHub.