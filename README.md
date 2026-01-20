# Inventory & Sales Tracking System

A professional portfolio project focused on an enterprise-ready backend built with .NET and Clean Architecture.

The system supports product inventory, sales creation, stock adjustments, and secure authentication using JWT with role-based authorization.

---

## Tech Stack

**Backend**
- .NET (net10.0)
- ASP.NET Core Web API

**Database**
- SQL Server
- Entity Framework Core (Code First + Migrations)

**Security**
- JWT Authentication
- Role-based Authorization (Admin / Clerk)
- PBKDF2 password hashing

**Validation & Testing**
- FluentValidation
- xUnit (Application unit tests)

**Tooling**
- Docker Compose (SQL Server)
- REST Client file for manual API testing
---

## Architecture Overview (Clean Architecture)

The solution follows Clean Architecture principles, separating concerns and enforcing clear dependency boundaries.

**Solution:** `InventorySalesSystem.sln`

### Projects

- **InventorySalesSystem.Domain**  
  Contains pure domain entities (no dependencies on EF Core, API, or Application).

- **InventorySalesSystem.Application**  
  Implements use cases (command/query handlers), contracts (DTOs), validation, domain events, and abstractions  
  (repositories, security, transactions).  
  This layer does **not** depend on Infrastructure or ASP.NET.

- **InventorySalesSystem.Infrastructure**  
  Provides technical implementations such as:
  - Entity Framework Core `DbContext`
  - Database migrations
  - Repository implementations
  - Transactions
  - Audit event handlers
  - Database seeding

- **InventorySalesSystem.Api**  
  ASP.NET Core Web API layer responsible only for HTTP concerns:
  - Controllers
  - Middleware
  - Authentication / Authorization
  - JWT token generation
  - Composition root (`Program.cs`)

- **InventorySalesSystem.Tests**  
  Unit tests for Application logic using fakes (no EF Core required).

This structure keeps business logic isolated, testable, and independent from frameworks.
---

## Backend Features

### Authentication & Authorization

- JWT Authentication
- Role-based authorization with two roles:
  - **Admin**
  - **Clerk**
- User registration and login
- Password hashing using PBKDF2
- Protected endpoints enforcing authorization rules

**Role rules:**
- **Clerk**
  - Can create sales
  - Cannot adjust product stock
- **Admin**
  - Can create sales
  - Can adjust product stock

### Authenticated User Profile

- `GET /api/auth/me`
- Returns the authenticated user profile:
  - `userId`
  - `email`
  - `role`
- Requires a valid JWT
- Used by the frontend to validate the session and render role-based UI

### Inventory & Sales

- Product inventory management
- Stock adjustment endpoint (Admin only)
- Sales creation
- Retrieve sales by ID
- Paginated sales listing

### Error Handling

- Global exception handling middleware
- Consistent HTTP error responses
---

## Database Seeding (Local / Development)

The backend includes an optional database seeding mechanism to ensure the system is immediately usable in local or development environments.

### Seeded Data (Optional)

- Initial **Admin** user
- Demo **Products**

Seeding is **idempotent** and safe to run multiple times.  
It will not duplicate data if records already exist.

### Configuration

Seeding is controlled via configuration:

```json
"Seed": {
  "Enabled": true,
  "Admin": {
    "Email": "admin@test.com",
    "Password": "Password123!"
  },
  "Products": {
    "Enabled": true
  }
}
```
- **Seed:Enabled**
    - Enables or disables database seeding.

- **Seed:Admin**
    - Initial admin credentials (local only).

- **Seed:Products**
    - Controls demo product creation.

- The password is hashed using the same PBKDF2 implementation used by the authentication flow.

### Important Notes

- Seeding is intended for local development and demos.

- It can be disabled in production environments via configuration.

- The seeding logic lives in the Infrastructure layer and is triggered from the API composition root.
---

## Getting Started (Local Development)

### Prerequisites

- .NET SDK (net10.0)
- Docker & Docker Compose
- (Optional) SQL Server client (SSMS or Azure Data Studio)

---

### Start SQL Server (Docker)

From the repository root, run:

```bash
docker compose up -d
```
The SQL Server container configuration is defined in ```docker-compose.yml```.

### Local Configuration

Create the following file:

```bash
InventorySalesSystem.Api/appsettings.Local.json
```

Use ```appsettings.Local.example.json``` as a reference.
This file is gitignored and should contain your real local values.

Minimal example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=InventorySalesDb;User Id=sa;Password=YOUR_STRONG_PASSWORD;Trust Server Certificate=True;"
  },
  "Jwt": {
    "Issuer": "InventorySalesSystem",
    "Audience": "InventorySalesSystem",
    "Key": "CHANGE_THIS_TO_A_LONG_RANDOM_SECRET_KEY_32+_CHARS"
  },
  "Seed": {
    "Enabled": true,
    "Admin": {
      "Email": "admin@test.com",
      "Password": "Password123!"
    },
    "Products": {
      "Enabled": true
    }
  }
}
```

### Apply Database Migrations

```bash
dotnet ef database update \
  --project InventorySalesSystem.Infrastructure \
  --startup-project InventorySalesSystem.Api
```

### Run the API
```bash
dotnet run --project InventorySalesSystem.Api
```

The API will start and be ready to accept requests.

---

## Testing

Unit tests are implemented for Application logic using xUnit.

Run all tests with:

```bash
dotnet test InventorySalesSystem.Tests
```

Tests focus on business rules and use fakes instead of EF Core to keep them fast and isolated.

### Manual API Testing
Tests focus on business rules and use fakes instead of EF Core to keep them fast and isolated.

```bash
http/inventory-api.rest
```

It includes requests to test:

- Login (Admin / Clerk)
- Token capture
- Protected endpoints
- Role-based authorization rules
- Authenticated user profile (/api/auth/me)
- This allows quick manual verification of authentication and authorization behavior.

## Roadmap

Planned next steps for the project:

1. Frontend with React + Tailwind
    - Login UI
    - JWT handling
    - Session validation using /api/auth/me
    - Role-based UI rendering
    - Sales listing and creation

2. Optional Backend Enhancements
    - Refresh tokens
    - Admin seeding improvements
    - Reports using SQL / Stored Procedures
    - Dockerizing the API
    - CI/CD pipelines with GitHub Actions

## License
This project is intended for learning and professional portfolio purposes.