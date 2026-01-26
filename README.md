# Inventory & Sales Tracking System

A full-stack, enterprise-style inventory and sales management system built as a professional portfolio project.

The system demonstrates a clean, scalable backend architecture with secure authentication, role-based authorization, and a modern React frontend with protected routes and role-aware UI.

It models a real-world scenario where **Admins manage products and inventory**, while **Clerks handle sales operations**, enforcing business rules at both API and UI levels.

---

## Tech Stack & Architecture

### Backend
- .NET (net10.0)
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- JWT Authentication & Role-based Authorization
- FluentValidation
- xUnit

### Frontend
- React + TypeScript
- Vite
- Tailwind CSS
- React Router (protected & role-based routes)

### Infrastructure & Tooling
- SQL Server (Docker Compose)
- REST Client for API testing
- Git Flow with protected main branch

### Architecture
- Clean Architecture with strict separation of concerns:
  - **Domain**: business entities and rules
  - **Application**: use cases, validation, contracts
  - **Infrastructure**: EF Core, persistence, implementations
  - **API**: controllers, middleware, authentication
- Business logic isolated and unit tested (no framework dependencies)

---
## Getting Started (Local Development)

### Prerequisites
- .NET SDK (net10.0)
- Docker & Docker Compose
- Node.js (18+)

---

### 1. Start SQL Server (Docker)

From the repository root:

```bash
docker compose up -d
```
---

### 2. Backend Configuration

Create the following file:

```bash
InventorySalesSystem.Api/appsettings.Local.json
```
Use appsettings.Local.example.json as reference.

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
    "Enabled": true
  }
}
```

### 3. Apply Database Migrations

```bash
dotnet ef database update \
  --project InventorySalesSystem.Infrastructure \
  --startup-project InventorySalesSystem.Api
```

### 4. Run Backend API

```bash
dotnet run --project InventorySalesSystem.Api
```

### 5. Run Frontend

```bash
cd frontend
npm install
npm run dev
```

The application will be available at:

Frontend: http://localhost:5173

Backend API: http://localhost:5000

---

## Demo Credentials

When database seeding is enabled, the following demo users are available:

**Admin**
- Email: `admin@test.com`
- Password: `Password123!`
- Permissions:
  - Manage products
  - Adjust stock
  - Create sales

**Clerk**
- Email: `clerk@test.com`
- Password: `Password123!`
- Permissions:
  - Create sales
  - View sales history

---

## Notes

- This project is designed as a professional portfolio application, focusing on clean architecture, security, and maintainability.
- Business rules are enforced at the API level and reflected in the frontend through role-based UI and protected routes.
- Database seeding is intended for local development and demo purposes only and can be disabled via configuration.
- The frontend follows a feature-based structure to mirror real-world team projects.

---

## Key Features

### Authentication & Authorization
- JWT-based authentication
- Role-based authorization (**Admin** / **Clerk**)
- Secure password hashing using PBKDF2
- Authenticated user profile endpoint (`/api/auth/me`)
- Frontend session restoration and route protection

### Inventory Management (Admin)
- Product catalog management
- Create products with validation
- Stock adjustment with audit reason
- Stock level protection (cannot go below zero)

### Sales Management
- Sales creation workflow
- Automatic stock deduction
- Business rule enforcement at API level
- Paginated sales listing

### Frontend Application
- React + TypeScript + Vite
- Role-aware UI rendering
- Protected routes (auth + role guards)
- Centralized API client
- Clean feature-based folder structure

### Backend Architecture
- Clean Architecture with strict layer separation
- Use-case driven application layer
- Centralized validation and error handling
- Unit-tested business logic
---

## License

This project is intended for learning and professional portfolio purposes.
