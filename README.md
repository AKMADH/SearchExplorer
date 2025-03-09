# SearchExplorer

Search Explorer API Documentation

Overview

Search Explorer is a .NET Core Web API that allows users to search and filter products based on various criteria. The API uses MediatR for handling requests, a repository pattern for database access, and a logging middleware to log requests and responses.

#Prerequisites

Before running the project, ensure you have the following installed:

1).NET Core SDK (latest version)

2)Visual Studio Code / Visual Studio

3)SQL Server 

4)Entity Framework Core Tools

5)Git


# Installation & Setup

1. Clone the Repository

Run the following command to clone the solution:

git clone https://github.com/AKMADH/SearchExplorer

2. Open in VS Code / Visual Studio

If using VS Code:

code .

If using Visual Studio, open the .sln file.

3. Configure Database Connection

Modify appsettings.json to add the database connection string:

"SearchExplorerApiSettings": {
  "ConnectionString": "Server=localhost,1433;Database=SearchExplorer;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;"
}

4. Run Entity Framework Migrations

# Run the following commands to create and apply database migrations:

dotnet restore
dotnet ef migrations add InitialCreate --project SearchExplorer.Infrastructure --startup-project SearchExplorer.Api
dotnet ef database update --project SearchExplorer.Infrastructure --startup-project SearchExplorer.Api

5. Build and Run the Solution

dotnet build
dotnet run --project SearchExplorer.Api

6. Open Swagger UI

Once the API is running, open the following URL in your browser:

http://localhost:5000/swagger

This will show all available endpoints and allow testing.

Authentication (JWT Token)

The API uses JWT authentication. To generate a token:

Go to the AuthController in Swagger (/api/auth)

Use the following credentials:

Username: testuser

Password: password123

The response will contain a JWT token.

Copy the token and include it in the Authorization header for authenticated requests.

Authorization: Bearer <your-token>

Product Controller

Endpoints

Search for Products by Keyword

GET https://localhost:5001/api/Product/search?Keyword=laptop

Filter Products by Price Range

GET https://localhost:5001/api/Product/search?Keyword=laptop&MinPrice=45000&MaxPrice=80000

Sort Products

GET [/api/product/sort?order=asc](https://localhost:5001/api/Product/search?Keyword=laptop&MinPrice=45000&MaxPrice=80000&SortBy=price&SortDirection=desc)

# Architecture & Design Patterns

MediatR: Handles requests via command and query handlers.

Repository Pattern: Provides abstraction for database access.

Middleware: Logs all incoming requests and outgoing responses.

Command Handler & Service Layer: Ensures separation of concerns.

Logging

The API includes middleware to log each request and response. Logs are stored in the configured logging provider.
<img width="1470" alt="Screenshot 2025-03-09 at 10 25 48 PM" src="https://github.com/user-attachments/assets/1dde5f70-d50a-424c-b3f4-4d6cc97b5708" />




