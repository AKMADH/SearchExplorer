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

Filter Products 
sort product 

# Architecture & Design Patterns

MediatR: Handles requests via command and query handlers.

Repository Pattern: Provides abstraction for database access i usedIn memory .

Middleware: Logs all incoming requests and outgoing responses.

Command Handler & Service Layer: Ensures separation of concerns.


# Endpoints
# 1. Authentication
POST /api/auth/login
Authenticate a user by sending their username and password as JSON in the request body.

# Request Body:

username: string (nullable)
password: string (nullable)
Responses:

200 OK: Authentication was successful.
Request Example:
# json

{
  "username": "testuser",
  "password": "password123"
}
# Response Example (200 OK):
json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidGVzdHVzZXIiLCJqdGkiOiI1MjUzMGRhYS04YWNlLTQyYTQtOGRhOS01NjJhN2M5NTEzMGQiLCJleHAiOjE3NDE2ODU3NjUsImlzcyI6InlvdXJfaXNzdWVyX2hlcmUiLCJhdWQiOiJ5b3VyX2F1ZGllbmNlX2hlcmUifQ.eV0uFNVV40sNowktrNIhYzNWas86M--sEfYE0RD9Umk"
}
# 2. Product Search
GET /api/Product/search
Search for products by providing search parameters as query parameters.

Query Parameters:

query: string (optional) — The search term for the product.
filter: integer (optional) — Filter for product type. This is an enumerated value:
0: Filter Type 0
1: Filter Type 1
2: Filter Type 2
3: Filter Type 3
public enum ProductFilterEnum
{
    Brand,
    Rating,
    Category,
    Price
}

filterValue: string (optional) — The value for the filter.
sort: integer (optional) — Sort order. This is an enumerated value:
0: Sort Type 0
1: Sort Type 1
2: Sort Type 2

public enum ProductSortEnum
{
    Name, 0
    Price, 1
    Rating 2
}
Responses:
{
  "statusCode": 200,
  "message": "Product details fetched successfully.",
  "data": {
    "products": [
      {
        "productId": 1,
        "name": "Laptop",
        "price": 1000,
        "stockQuantity": 10,
        "description": "High performance laptop",
        "category": "Electronics",
        "brand": "BrandA",
        "rating": 4.5
      }
    ],
    "totalCount": 1
  }
}
200 OK: A list of products matching the search criteria.

#The API includes middleware to log each request and response. Logs are stored in the configured logging provider.


![image](https://github.com/user-attachments/assets/5c571c51-b23f-434e-9cf0-003ba3147a7b)

