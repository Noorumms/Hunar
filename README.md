# Hunar — Hyperlocal Services Marketplace

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/EF%20Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white"/>
  <img src="https://img.shields.io/badge/JWT-Auth-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white"/>
  <img src="https://img.shields.io/badge/Clean%20Architecture-✓-28a745?style=for-the-badge"/>
</p>

<p align="center">
  <b>Connecting customers with verified local skilled workers across Pakistani cities.</b><br/>
  <i>"Har Kaam Ka Ustaad" — A Master for Every Job</i>
</p>

---

## Overview

Hunar is a hyperlocal skilled worker marketplace REST API built with ASP.NET Core following Clean Architecture principles. It solves a real problem in Pakistan — finding reliable local service providers (electricians, plumbers, tutors, AC technicians, carpenters) is fragmented across WhatsApp groups, Facebook, and word of mouth. Hunar centralizes discovery, booking, and trust in one platform.

---

## Architecture

This project strictly follows **Clean Architecture** with a 4-layer separation of concerns. Dependencies always point inward — toward the Domain.
┌─────────────────────────────────────────────┐

│                  Hunar.API                  │  ← HTTP, Controllers, Middleware, JWT

├─────────────────────────────────────────────┤

│             Hunar.Application               │  ← Use Cases, Business Logic, Events

├─────────────────────────────────────────────┤

│             Hunar.Infrastructure            │  ← EF Core, Repositories, DbContext

├─────────────────────────────────────────────┤

│               Hunar.Domain                  │  ← Entities, Interfaces, Enums (no dependencies)

└─────────────────────────────────────────────┘

| Layer | Responsibility |
|---|---|
| `Hunar.Domain` | Core entities, repository interfaces, enums. Zero external dependencies. |
| `Hunar.Application` | Use cases, business rules, delegates, events, DTOs. Depends only on Domain. |
| `Hunar.Infrastructure` | EF Core DbContext, repository implementations, migrations. Depends on Domain. |
| `Hunar.API` | ASP.NET Core controllers, middleware pipeline, JWT config, DI registration. |

---

## Tech Stack

| Concern | Technology |
|---|---|
| Framework | ASP.NET Core Web API (.NET 8) |
| ORM | Entity Framework Core 8 (Code First) |
| Database | Microsoft SQL Server (LocalDB for dev) |
| Authentication | JWT Bearer Tokens |
| Authorization | Role-based (Customer, Provider, Admin) |
| API Docs | Swagger / OpenAPI |
| Frontend | HTML5, CSS3, Vanilla JavaScript |
| AI Layer | OpenAI API — smart provider matching *(planned)* |
| Deployment | Railway / Azure App Service *(planned)* |

---

## Domain Model
User ────────────────── ProviderProfile

│                            │

│                     ServiceListing (many)

│                            │

└──── ServiceRequest ────────┘

│

Review

| Entity | Description |
|---|---|
| `User` | Any registered account — Customer, Provider, or Admin |
| `ProviderProfile` | Extended profile for service providers with location and category |
| `ServiceListing` | A specific service offered by a provider with title and price |
| `ServiceRequest` | Core transaction — customer requests a service from a provider |
| `Review` | Rating and comment left after a completed job |

### Booking Workflow
[Pending] ──► [Confirmed] ──► [Completed]

│               │

└───────────────┴──► [Cancelled]

---

## Features

### Customer
- Browse and search providers by category, city, and area
- View provider profiles with listings, ratings, and availability
- Submit service requests with preferred date and description
- Track booking status in real time
- Leave a review after job completion

### Provider
- Create and manage a service profile
- Define service listings with custom pricing
- Accept, reject, or cancel incoming requests
- View booking history and earnings

### Admin *(in progress)*
- Approve or suspend provider accounts
- Moderate reviews and reported disputes
- View platform-wide usage metrics

### Platform
- JWT authentication with role-based access control
- Request logging middleware — every endpoint timestamped
- Global exception handling middleware — consistent JSON error responses
- Delegates and events — booking state changes fire decoupled notifications
- LINQ throughout — no manual loops in query logic

---

## Project Structure
Hunar/

├── Hunar.Domain/

│   ├── Entities/

│   │   ├── User.cs

│   │   ├── ProviderProfile.cs

│   │   ├── ServiceListing.cs

│   │   ├── ServiceRequest.cs

│   │   └── Review.cs

│   ├── Interfaces/

│   │   ├── IUserRepository.cs

│   │   ├── IProviderRepository.cs

│   │   ├── IServiceRequestRepository.cs

│   │   └── IReviewRepository.cs

│   └── Enums/

│       └── Enums.cs

│

├── Hunar.Application/

│   ├── UseCases/

│   │   ├── Auth/

│   │   ├── Provider/

│   │   ├── ServiceRequest/

│   │   └── Review/

│   ├── DTOs/

│   ├── Delegates/

│   └── Events/

│

├── Hunar.Infrastructure/

│   ├── Data/

│   │   ├── HunarDbContext.cs

│   │   └── HunarDbContextFactory.cs

│   ├── Repositories/

│   └── Migrations/

│

└── Hunar.API/

├── Controllers/

├── Middleware/

├── appsettings.json

└── Program.cs

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or LocalDB (included with Visual Studio)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### Setup

```bash
# Clone the repository
git clone https://github.com/yourusername/Hunar.git
cd Hunar
```

Update the connection string in `Hunar.API/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HunarDb;Trusted_Connection=True;"
}
```

```bash
# Apply database migrations
# In Package Manager Console (Default project: Hunar.Infrastructure)
Update-Database -StartupProject Hunar.API
```

```bash
# Run the API
# Set Hunar.API as startup project and press F5
# Swagger UI will open at https://localhost:{port}/swagger
```

---

## API Endpoints *(growing daily)*

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `POST` | `/api/auth/register` | Public | Register as customer or provider |
| `POST` | `/api/auth/login` | Public | Login and receive JWT token |
| `GET` | `/api/providers` | Public | Browse all providers |
| `GET` | `/api/providers/{id}` | Public | View provider profile |
| `POST` | `/api/servicerequests` | Customer | Submit a service request |
| `PUT` | `/api/servicerequests/{id}/confirm` | Provider | Accept a request |
| `PUT` | `/api/servicerequests/{id}/complete` | Provider | Mark job as done |
| `POST` | `/api/reviews` | Customer | Leave a review |
| `GET` | `/api/admin/users` | Admin | View all users |

---

## Development Progress

- [x] Day 1 — Solution setup, Clean Architecture, Domain entities, EF Core, DB migration
- [ ] Day 2 — JWT Authentication, Register, Login, Role-based access
- [ ] Day 3 — Provider profiles, Service listings, Repository implementations
- [ ] Day 4 — Customer flow, Service request booking workflow
- [ ] Day 5 — Reviews, Admin endpoints, Delegates and Events
- [ ] Day 6 — Middleware, LINQ optimization, Global error handling
- [ ] Day 7 — Frontend (HTML/CSS/JS connected to API)
- [ ] Day 8 — AI provider matching feature
- [ ] Day 9 — Deployment (Railway / Azure)

---

## Why This Project

Built as a portfolio project to demonstrate real-world .NET development practices:
- Clean Architecture with strict dependency rules
- Code-first database design with EF Core migrations
- Secure JWT authentication with role separation
- Publisher-Subscriber pattern through C# events and delegates
- Async/await throughout for scalable I/O
- Progressively enhanced with an AI layer

---

## License

MIT License — feel free to use this as reference or inspiration.

---

<p align="center">Built by <a href="https://github.com/Noorumms">Noor Fatima</a> · Pakistan 🇵🇰</p>
