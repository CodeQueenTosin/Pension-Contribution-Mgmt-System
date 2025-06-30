🏦 Pension Contribution Management System-(PCMS)
A robust Pension Contribution Management System built with ASP.NET Core 8.0, leveraging Clean Architecture, Entity Framework Core, and Hangfire for efficient pension tracking and management. This project simplifies member management, contribution tracking, and automated background processing, ensuring a seamless experience for managing Pension Contributions.

✅ Features

Member Management: Register, update, and soft-delete members with ease.
Contribution Tracking: Track contributions and generate detailed statements.
Data Seeding: Pre-populate employer and benefit data for quick setup.
Automated Background Jobs: Validates monthly contributions using Hangfire.
API Documentation: Interactive Swagger UI for easy API exploration.
Job Monitoring: Hangfire dashboard to monitor and manage background tasks.
Clean Architecture: Organized into Domain, Application, and Infrastructure layers for maintainability.


🧰 Technologies Used

ASP.NET Core 8.0: Modern web framework for building APIs.
Entity Framework Core: ORM for seamless database interactions (SQL Server).
Hangfire: Background job processing and scheduling.
FluentValidation: Robust validation for input data.
xUnit: Unit testing framework for reliable code.
Swagger: API documentation and testing interface.


✅ Prerequisites
Ensure the following tools are installed before setting up the project:


Installation

.NET 8 SDK
Build and run the application
Download from the official site


SQL Server
Relational database for data storage
Install Express or Developer edition


EF Core CLI
Manage database migrations
dotnet tool install --global dotnet-ef



🚀 Step-by-Step Setup Instructions
Follow these steps to get the project up and running:
1️⃣ Clone the Repository
Clone the project and navigate to the directory:
git clone https://github.com/codequeentosin/pension-contribution-system.git
cd pension-contribution-system

2️⃣ Open the Project
Open the project in your preferred IDE:

Visual Studio: Open the .sln file.
VS Code: Run code . in the terminal.

3️⃣ Restore Dependencies
Restore the required NuGet packages:
dotnet restore

4️⃣ Configure the Connection String
Update the database connection string in appsettings.json or appsettings.Development.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PensionDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
Replace Server, Database, and credentials as needed for your SQL Server setup.

5️⃣ Apply Database Migrations
If the Migrations/ folder does not exist, create the initial migration:
dotnet ef migrations add InitialCreate

Apply the migration to create the database:
dotnet ef database update

6️⃣ (Optional) Generate SQL Script
To generate a SQL script for manual database setup:
dotnet ef migrations script -o MigrationScript.sql

7️⃣ Run the Application
Launch the application:
dotnet run

8️⃣ Access the Application

Swagger UI: https://localhost:{port}/swagger (e.g., https://localhost:5001/swagger)
Hangfire Dashboard: https://localhost:{port}/hangfire (e.g., https://localhost:5001/hangfire)


Note: Replace {port} with the port number displayed in the console (typically 5000 or 5001).


🧪 Running Tests
The project includes unit tests built with xUnit. To run the tests:
dotnet test

This executes all tests in the solution and provides a summary of the results.

🛠️ Troubleshooting

Database Connection Issues: Ensure SQL Server is running and the connection string in appsettings.json is correct.
Migration Errors: Verify the EF Core CLI is installed (dotnet ef --version) and the connection string is valid.
Hangfire Dashboard Access: Ensure you have the correct credentials (if authentication is enabled) and the dashboard URL is correct.
Port Conflicts: If the default port is occupied, update the port in Properties/launchSettings.json.


📚 API Documentation

The API is documented using Swagger UI, accessible at https://localhost:{port}/swagger. Key endpoints include:

Endpoint

Method

Description

/api/members
POST
Register a new member


/api/members/{id}
GET
Retrieve member details

/api/members/{id}
PUT
Update member information

/api/members/{id}
DELETE
Soft-delete a member

/api/contributions
POST
Record a contribution

/api/contributions/statements/{memberId}
GET
Generate contribution statement

🏛️ Architecture Overview

The project follows Clean Architecture principles, organized into three layers:

Domain Layer: Contains business entities (e.g., Member, Contribution) and interfaces for repositories and services. Independent of external frameworks.

Application Layer: Houses business logic, DTOs, and application services. Orchestrates use cases like member registration and contribution tracking.

Infrastructure Layer: Implements data access (EF Core), external services (Hangfire), and configuration (appsettings.json).

This separation ensures maintainability, testability, and scalability.


🧠 Design Decisions Explanation

Clean Architecture: Chosen for modularity and testability. It isolates business logic from infrastructure, making the system easier to maintain and extend.

Entity Framework Core: Selected for its robust ORM capabilities, simplifying database operations and migrations.

Hangfire: Used for background job processing (e.g., monthly contribution validation) due to its reliability and integration with ASP.NET Core.

FluentValidation: Provides a clean, maintainable way to validate input data, reducing boilerplate code.

SQL Server: Chosen for its enterprise-grade features and compatibility with EF Core.

Swagger: Included for interactive API documentation, improving developer experience.

Soft Deletes: Implemented to preserve historical data while allowing logical deletion of members.

These decisions balance performance, scalability, and developer productivity.


📞 Contact
Author: Tosin Olorunnisola Email: codequeentosin@gmail.com GitHub: CodeQueenTosin
Feel free to reach out for setup assistance, feature requests, or contributions!

🤝 Contributing
Contributions are welcome! To contribute:

Fork the repository.
Create a feature branch (git checkout -b feature/YourFeature).
Commit your changes (git commit -m "Add YourFeature").
Push to the branch (git push origin feature/YourFeature).
Open a Pull Request.

Please ensure your code follows the project's coding standards and includes tests.

Happy Coding!
