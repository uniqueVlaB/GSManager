# 🌿 GSManager — Garden Society Manager

A full-stack web application for managing a garden society (allotment cooperative). It handles members, land plots, electricity meters, user accounts, and role-based access control — all through a clean REST API and an Angular SPA.

---

### Projects

| Project | Role |
|---|---|
| `GSManager` | .NET Aspire AppHost — orchestrates all services |
| `AspireServiceDefaults` | Shared Aspire service configuration |
| `GSManager.API` | ASP.NET Core Web API — controllers, middleware, auth config |
| `GSManager.Core` | Business logic — services, domain models, validators, filter pipelines |
| `GSManager.Infrastructure.SQL` | EF Core data access, migrations, Unit of Work |
| `GSManager.Infrastructure.Mailer` | SMTP email delivery infrastructure (used by the Mailer service) |
| `GSManager.Mailer` | .NET Worker Service — MassTransit consumer that processes email events from RabbitMQ |
| `GSManager.Contracts` | Shared message contracts (events) exchanged between API and Mailer |
| `GSManager.Angular` | Angular SPA — UI, routing, signal-based state |

---

## ✨ Features

### 🏡 Society Management
- **Members** — full CRUD with pagination and filtering
- **Plots** — land plot management, assignable to members
- **Electricity Meters** — per-plot meter tracking with installation dates, maintenance history, owner assignment, and multi-field filtering

### 🔐 Security & Identity
- JWT Bearer authentication with **in-memory token storage** on the client (no `localStorage`)
- **HttpOnly cookie** refresh tokens with optional "Remember Me" persistence
- Email confirmation flow — confirmation emails are dispatched asynchronously via **RabbitMQ** and processed by the dedicated `GSManager.Mailer` worker
- Granular **permission-based authorization** (RBAC) — e.g. `electricity_meters:add`, `plots:edit`
- Role and privilege management

### 🛠️ Developer Experience
- **Scalar** interactive API docs (dev only) with OAuth2 password flow for auto token injection
- **.NET Aspire** orchestration and distributed telemetry
- **Serilog** structured logging
- **FluentValidation** on all request DTOs
- Composable **filter pipelines** per entity
- Local config override via `appsettings.Local.json`

---

## 🧰 Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 10 / C# 14 |
| API Framework | ASP.NET Core |
| ORM | Entity Framework Core |
| Database | PostgreSQL (Aspire container) |
| Messaging | RabbitMQ + MassTransit |
| Validation | FluentValidation |
| Logging | Serilog |
| Observability | OpenTelemetry (.NET Aspire) |
| API Docs | Scalar + Microsoft.AspNetCore.OpenApi |
| Frontend | Angular (Signals) |
| Styling | SCSS (CSS custom properties) |
| Auth (client) | jwt-decode, Angular Signals |

---

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js (LTS) + npm](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (required for Aspire containers — PostgreSQL, RabbitMQ)
- [.NET Aspire workload](https://learn.microsoft.com/dotnet/aspire/fundamentals/setup-tooling)

```powershell
# Install Aspire workload (once)
dotnet workload install aspire
```

### 1 — Clone the repository

```powershell
git clone https://github.com/uniqueVlaB/GSManager.git
cd GSManager
```

### 2 — Configure local secrets

All secrets are stored in the **AppHost user secrets** (`GSManager/GSManager.csproj`). Set them once with:

```powershell
cd GSManager
dotnet user-secrets set "Parameters:JwtSecretKey"                    "<min-32-character-secret>"
dotnet user-secrets set "Parameters:JwtIssuer"                       "GSManagerAPI"
dotnet user-secrets set "Parameters:JwtAudience"                     "GSManagerClient"
dotnet user-secrets set "Parameters:JwtExpirationInMinutes"          "15"
dotnet user-secrets set "Parameters:JwtRefreshTokenExpirationInDays" "7"
dotnet user-secrets set "Parameters:MailerServer"                    "smtp.gmail.com"
dotnet user-secrets set "Parameters:MailerPort"                      "587"
dotnet user-secrets set "Parameters:MailerSenderName"                "GSManager"
dotnet user-secrets set "Parameters:MailerSenderEmail"               "your-email@gmail.com"
dotnet user-secrets set "Parameters:MailerUsername"                  "your-email@gmail.com"
dotnet user-secrets set "Parameters:MailerPassword"                  "<app-password>"
dotnet user-secrets set "Parameters:MailerFrontendBaseUrl"           "http://localhost:4300"
```

> PostgreSQL and RabbitMQ are provisioned automatically as Docker containers by .NET Aspire — no manual setup needed.

### 3 — Run via .NET Aspire (recommended)

```powershell
dotnet run --project GSManager/GSManager.csproj
```

Aspire starts the API, installs Angular npm packages, and launches the Angular dev server automatically.

> The database schema is created and seeded automatically on first startup in the `Development` environment.

### 4 — Access the application

| Service | URL |
|---|---|
| Angular SPA | http://localhost:4300 |
| ASP.NET Core API | https://localhost:\<aspire-assigned-port\> |
| Scalar API docs | https://localhost:\<api-port\>/scalar/v1 |
| Aspire dashboard | http://localhost:15888 |

> **Tip:** Once the API is healthy, click the **"Open Scalar UI Documentation"** command button directly from the Aspire dashboard to launch Scalar in one click.

---

## 🗂️ Project Structure

```
GSManager/
├── GSManager/                           # .NET Aspire AppHost
├── AspireServiceDefaults/               # Shared Aspire defaults
├── GSManager.Contracts/                 # Shared MassTransit message contracts (events)
├── GSManager.Backend/
│   ├── GSManager.API/                   # Controllers, middleware, DI, auth config
│   │   ├── Controllers/
│   │   │   ├── Auth/                    # Login, refresh, confirm-email, logout
│   │   │   ├── Electricity/             # Electricity meter endpoints
│   │   │   └── Society/                 # Members, plots, roles, users, privileges
│   │   └── Config/                      # Auth, CORS, Serilog, Scalar configuration
│   ├── GSManager.Core/                  # Business logic layer
│   │   ├── Abstractions/                # Service & repository interfaces
│   │   ├── Auth/                        # Permissions, JWT claims, authorization handler
│   │   ├── Filters/                     # Composable server-side filter pipelines
│   │   ├── FluentValidation/            # DTO validators
│   │   ├── Mappers/                     # Entity ↔ DTO mapping
│   │   ├── Models/DTOs/                 # Request, response, filter, and entity DTOs
│   │   └── Services/                    # Auth, Society, Electricity implementations
│   ├── GSManager.Infrastructure/
│   │   ├── GSManager.Infrastructure.SQL/     # EF Core, repositories, Unit of Work, migrations
│   │   └── GSManager.Infrastructure.Mailer/ # SMTP email delivery (used by Mailer service)
│   └── GSManager.Mailer/                # Worker Service — MassTransit consumers, email sending
│       ├── Consumers/                   # SendEmailConsumer, EmailConfirmationConsumer
│       └── Templates/                   # HTML email templates
└── GSManager.Angular/
    └── src/app/
        ├── core/                        # Auth service, HTTP interceptors, guards, signals
        ├── features/
        │   ├── auth/                    # Login page
        │   ├── home/                    # Dashboard
        │   ├── members/                 # Member management
        │   ├── plots/                   # Plot management
        │   ├── user/                    # User profile
        │   └── state-pages/             # 404, access-denied
        └── shared/                      # Models, enums, shared UI components
```

---

## 🔑 Permissions Reference

Permissions follow the `resource:action` naming convention and are embedded in the JWT as claims.

| Resource | `view` | `add` | `edit` | `delete` |
|---|---|---|---|---|
| Members | `members:view` | `members:add` | `members:edit` | `members:delete` |
| Plots | `plots:view` | `plots:add` | `plots:edit` | `plots:delete` |
| Electricity Meters | `electricity_meters:view` | `electricity_meters:add` | `electricity_meters:edit` | `electricity_meters:delete` |
| Users | `users:view` | `users:add` | `users:edit` | `users:delete` |
| Roles | `roles:view` | `roles:add` | `roles:edit` | `roles:delete` |
| Privileges | `priviledges:view` | `priviledges:add` | `priviledges:edit` | `priviledges:delete` |
| — | `full_access` (superuser — bypasses all policy checks) | | | |

---

## 📄 License

This project is private. All rights reserved © [uniqueVlaB](https://github.com/uniqueVlaB).
