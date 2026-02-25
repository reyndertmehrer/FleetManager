# FleetManager 🚗

ASP.NET Core Web API for managing vehicles, using Entity Framework Core with PostgreSQL.

## 🧱 Solution Structure

* **FleetManager.Api** — ASP.NET Core Web API
* **FleetManager.Services** — Service layer (business logic, interfaces)
* **FleetManager.DAL** — Data Access Layer (EF Core, entities, DbContext)
* **FleetManager.Tests** — Test project containing unit tests and integration tests
* **PostgreSQL** — Database (Docker)

## ⚙️ Requirements

* .NET 10 SDK
* Docker Desktop

## 🐘 Run PostgreSQL with Docker

```bash
docker compose up -d
```

Database will be available at:

```
Host=localhost
Port=5432
Database=FleetManagerDb
```

## 🛠️ Apply EF Core Migrations

```bash
dotnet ef database update \
  --project FleetManager.DAL \
  --startup-project FleetManager.Api
```

## ▶️ Run the API

```bash
dotnet run --project FleetManager.Api
```

API will start at:

```
https://localhost:5012
```

## 📚 Tech Stack

* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Docker

---

I opted for a Service-only layer to avoid the redundancy of wrapping EF Core's built-in Repository pattern, prioritizing YAGNI for this specific scope. 
However, for a larger system requiring data source abstraction, I would implement a Repository pattern.
