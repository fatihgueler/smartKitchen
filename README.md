# SmartKitchen

SmartKitchen is a full-stack .NET 8 application for running core kitchen workflows in one place. It combines recipe management, ingredient and inventory tracking, weekly meal planning, shopping list generation, and order handling behind a Blazor Server UI and an ASP.NET Core Web API.

The repository is organized as a layered solution with separate projects for the frontend, API, domain model, and infrastructure. The current UI language is German.

## Overview

SmartKitchen is designed around a practical operational flow:

- define and maintain recipes
- manage ingredients and stock levels
- build weekly meal plans
- generate shopping lists from planned meals and current inventory
- track orders and summarize activity on a dashboard

The API uses Entity Framework Core with SQLite and applies pending migrations automatically on startup. The solution also ships with seed data so the application is usable immediately in local development.

## Core Features

- Dashboard with high-level kitchen metrics, recent orders, and today's meals
- Recipe management with preparation time, cooking time, servings, difficulty, and estimated cost
- Ingredient catalog and inventory tracking
- Low-stock and expiring-item monitoring
- Weekly meal planning
- Shopping list generation from meal plans minus current stock
- Order management
- Swagger-enabled API documentation in development

## Architecture

```text
Blazor Server UI (SmartKitchen)
        |
        v
ASP.NET Core Web API (SmartKitchen.API)
        |
        v
Entity Framework Core + SQLite
        |
        v
Domain and Infrastructure projects
```

This is a layered .NET solution rather than a single monolithic project file. Responsibilities are separated so UI, API, domain models, and persistence concerns can evolve independently.

## Solution Structure

```text
SmartKitchen/
|- SmartKitchen.csproj               # Blazor Server frontend
|- Components/                      # Razor components and pages
|- SmartKitchen.API/                # ASP.NET Core Web API
|- SmartKitchen.Domain/             # Domain entities
|- SmartKitchen.Application/        # Application layer
|- SmartKitchen.Infrastructure/     # EF Core DbContext and migrations
|- wwwroot/                         # Static assets
`- SmartKitchen.sln                 # Solution entry point
```

## Technology Stack

| Area | Technology |
| --- | --- |
| Frontend | Blazor Server (.NET 8) |
| Backend | ASP.NET Core Web API |
| Persistence | Entity Framework Core |
| Database | SQLite |
| API Docs | Swagger / OpenAPI |
| Tooling | .NET SDK 8 |

## Local Development

### Prerequisites

- .NET 8 SDK

### Restore dependencies

```powershell
dotnet restore .\SmartKitchen.sln
```

### Start the API

```powershell
dotnet run --project .\SmartKitchen.API\SmartKitchen.API.csproj
```

### Start the frontend

```powershell
dotnet run --project .\SmartKitchen.csproj
```

### Default local URLs

- Frontend: `http://localhost:5037`
- API: `http://localhost:5011`
- Swagger: `http://localhost:5011/swagger`

If you start the solution from Rider or Visual Studio, configure the frontend and API as startup projects so both processes run together.

## Configuration Notes

The frontend currently configures its `HttpClient` with a fixed API base address in `Program.cs`:

- `http://localhost:5011`

If you change the API port, update the frontend configuration accordingly.

## Database and Seed Data

- SQLite is used for local persistence
- The connection string points to `SmartKitchen.db`
- EF Core migrations are stored in `SmartKitchen.Infrastructure/Migrations`
- Pending migrations are applied automatically when the API starts
- Seed data includes sample ingredients, recipes, and inventory records

Local database files are excluded from source control via `.gitignore`.

## API Surface

The current API exposes endpoints for:

- `api/dashboard`
- `api/recipes`
- `api/ingredients`
- `api/inventory`
- `api/mealplans`
- `api/orders`
- `api/shoppinglist`

Swagger is available in development for endpoint discovery and manual testing.

## Build

```powershell
dotnet build .\SmartKitchen.sln
```

## Repository Notes

- IDE metadata is excluded from version control
- Build output folders are excluded from version control
- Local SQLite files are excluded from version control

## Status

The repository already contains the main application structure and a working feature set for local development. It is a strong foundation for extending the kitchen workflow, hardening API validation, and adding automated tests or deployment automation.
