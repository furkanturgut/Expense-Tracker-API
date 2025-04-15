Expense Tracker API
===================

Overview
--------

The Expense Tracker API is a RESTful service designed to help users track and manage their expenses. Built with ASP.NET Core, this API provides a robust backend for expense management applications, offering secure authentication and comprehensive expense tracking capabilities.

Features
--------

*   **User Authentication**: Secure login and registration with JWT token implementation
    
*   **Expense Management**: Create, read, update, and delete expense records
    
*   **Categorization**: Organize expenses by custom categories
    
*   **Filtering & Sorting**: Find expenses by date ranges, categories, or amounts
    
*   **Reporting**: Generate summaries of spending patterns
    
*   **Data Seeding**: Initial database population with sample data
    

API Endpoints
-------------

### Authentication

*   POST /api/account/register - Register a new user
    
*   POST /api/account/login - Log in and receive JWT authentication token
    

### Expenses

*   GET /api/expenses - Get all expenses for authenticated user
    
*   GET /api/expenses/{id} - Get a specific expense by ID
    
*   POST /api/expenses - Create a new expense
    
*   PUT /api/expenses/{id} - Update an existing expense
    
*   DELETE /api/expenses/{id} - Delete an expense
    

Getting Started
---------------

### Prerequisites

*   .NET 6.0 or higher
    
*   SQL Server (or alternative database configured in appsettings.json)
    

### Installation

1.  git clone https://github.com/yourusername/Expense-Tracker-API.git
    
2.  dotnet restore
    
3.  Update the database connection string in appsettings.json.
    
4.  dotnet ef database update
    
5.  dotnet run
    

Technology Stack
----------------

*   ASP.NET Core Web API
    
*   Entity Framework Core
    
*   SQL Server
    
*   JWT Authentication (Token-based security)
    
*   Data Seeding for initial application state


## Test Coverage

This project includes unit tests and integration tests to ensure reliability and maintainability. The following libraries are used for testing:

- **xUnit**: A widely used testing framework for .NET applications.
- **FakeItEasy**: A library for creating mock objects to simulate dependencies.
- **FluentAssertions**: A library for writing more readable and expressive assertions.

### Running Tests

To run the tests, use the following command in the terminal:
```bash
dotnet test



### Key Learnings

This project was a great opportunity to learn and implement:

- **JWT Authentication**: Implemented secure token-based authentication for the first time.
- **Data Seeding**: Learned how to populate the database with initial data for testing and development purposes.
- **Entity Framework Core**: Used EF Core for database interactions, including migrations and LINQ queries.
- **RESTful API Design**: Designed and implemented RESTful endpoints following best practices.
- **Testing**: Gained experience using **xUnit**, **FakeItEasy**, and **FluentAssertions** to write unit tests and integration tests, ensuring code reliability and maintainability.
    

Documentation
-------------

API documentation is available via Swagger UI when running the application in development mode at /swagger.

License
-------

This project is licensed under the MIT License - see the LICENSE file for details.

Contributing
------------

Contributions are welcome! Please feel free to submit a Pull Request.
