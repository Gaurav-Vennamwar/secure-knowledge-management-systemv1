<div align="center">

<img src="https://raw.githubusercontent.com/Gaurav-Vennamwar/secure-knowledge-management-systemv1/main/SecureKnowledgeManagement.ui/src/assets/logo.png" width="80" alt="SKMS Logo" />

# Secure Knowledge Management System

**A production-grade full-stack blog & knowledge platform built with enterprise architecture patterns**


Why This Project?

✔ Enterprise Authentication

✔ Clean Architecture

✔ Repository Pattern

✔ Production Deployment

✔ Modern Angular

✔ Secure JWT Authentication

✔ Refresh Token Rotation

✔ Azure SQL Database

✔ Cloud Hosting

✔ Responsive UI

✔ Markdown Editor

✔ Pagination

✔ Role-Based Authorization

[![Angular](https://img.shields.io/badge/Angular-20-DD0031?style=flat-square&logo=angular)](https://angular.dev)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoftsqlserver)](https://microsoft.com/sql-server)
[![EF Core](https://img.shields.io/badge/EF_Core-8.0-512BD4?style=flat-square&logo=dotnet)](https://learn.microsoft.com/ef/core)
[![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=flat-square&logo=jsonwebtokens)](https://jwt.io)
[![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)](LICENSE)

## Project Status

✅ Production Ready

✅ Live Deployment

✅ Enterprise Authentication

✅ Refresh Token Rotation

✅ Role-Based Authorization

✅ Responsive Design

🚧 Continuous Improvements

Currently Working On

• Forgot Password

• Reading Progress

• Better Dashboard

• Search Improvements

• Learning Paths

• Premium UI Polish

## 🌐 Live Applications

🚀 **SKMS Live Application**
https://secure-knowledge-management-systemv-ten.vercel.app/

💼 **Developer Portfolio**
https://gaurav-portfolio-woad.vercel.app/

🐞 **Report Issues**
https://github.com/Gaurav-Vennamwar/secure-knowledge-management-systemv1/issues

💼 **LinkedIn**
https://www.linkedin.com/in/gaurav-vennamwar-0b79b0212

</div>

---

## What is SKMS?

SKMS is a **production-ready knowledge management and blogging platform** designed from the ground up with enterprise architecture in mind. It is not a tutorial clone — every architectural decision was intentional, from HttpOnly cookie-based token storage to refresh token rotation and global exception middleware.

The project follows a **5-phase roadmap** moving from a monolithic fullstack application toward microservices, Docker containerization, and AI-powered content features.

> Built to demonstrate real-world backend depth, secure authentication patterns, and clean Angular architecture — not just CRUD.

---

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                        CLIENT LAYER                          │
│   Angular 20 · Signals · httpResource · HTTP Interceptor     │
└─────────────────────┬───────────────────────────────────────┘
                      │ HTTP + HttpOnly Cookies
┌─────────────────────▼───────────────────────────────────────┐
│                        API LAYER                             │
│   ASP.NET Core 8 · REST · JWT · Rate Limiting · CORS        │
│                                                              │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Auth         │  │ BlogPosts    │  │ Categories       │  │
│  │ Controller   │  │ Controller   │  │ Controller       │  │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────────┘  │
│         │                 │                  │               │
│  ┌──────▼─────────────────▼──────────────────▼───────────┐  │
│  │              Repository Pattern Layer                  │  │
│  │   ICategoryRepository · IBlogPostRepository           │  │
│  │   ITokenRepository    · IImageRepository              │  │
│  └──────────────────────────┬────────────────────────────┘  │
│                             │                                │
│  ┌──────────────────────────▼────────────────────────────┐  │
│  │              Middleware Pipeline                       │  │
│  │   GlobalExceptionMiddleware · Authentication           │  │
│  │   Authorization · Rate Limiter · CORS · Serilog        │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────┬───────────────────────────────────────┘
                      │ EF Core
┌─────────────────────▼───────────────────────────────────────┐
│                      DATA LAYER                              │
│   SQL Server · ApplicationDbContext · AuthDbContext          │
│   RefreshTokens · AspNetUsers · BlogPosts · Categories       │
└─────────────────────────────────────────────────────────────┘
```

---

## Feature Highlights

### Security & Authentication
| Feature | Implementation |
|---|---|
| JWT Authentication | HttpOnly cookies · 15-minute expiry · HS256 signing |
| Refresh Token Rotation | Every use invalidates old token · generates new · 7-day window |
| Role-Based Access Control | Reader / Writer roles · Angular route guards · ASP.NET `[Authorize]` |
| XSS Protection | Tokens inaccessible to JavaScript via HttpOnly |
| CSRF Protection | SameSite=Lax cookie policy |
| Rate Limiting | Built-in .NET 8 fixed window limiter · 100 req/min on auth endpoints |
| Password Security | BCrypt hashing via ASP.NET Identity · salted per user |

### Backend Architecture
| Feature | Implementation |
|---|---|
| Repository Pattern | Clean separation of data access from business logic |
| Global Exception Middleware | Catches all unhandled exceptions · returns consistent JSON |
| API Response Wrapper | `ApiResponse<T>` — every endpoint returns `{ Success, Message, Data, StatusCode }` |
| Server-side Pagination | `Skip/Take` with `TotalCount`, `TotalPages` metadata |
| Structured Logging | Serilog · console + rolling daily file output |
| Environment Config | `appsettings.Development.json` / `appsettings.Production.json` separated |
| Dual DbContext | `ApplicationDbContext` for content · `AuthDbContext` for identity |
| Data Seeding | EF Core `HasData()` · roles and admin user seeded on migration |

### Frontend Architecture
| Feature | Implementation |
|---|---|
| Angular Signals | `signal()` · `httpResource()` · reactive state without RxJS complexity |
| HTTP Interceptor | Auto-attaches `withCredentials` · silently refreshes JWT on 401 |
| Reactive Forms | `FormGroup` · `FormControl` · validation with error display |
| Markdown Support | `ngx-markdown` · full markdown rendering in blog content |
| Image Management | Upload endpoint · `PhysicalFileProvider` · URL stored in DB |
| Rich Category UI | `ng-select` multi-select for category assignment on blog posts |
| Pagination Controls | Previous/Next with `Page X of Y` · signal-driven auto-refetch |


                Users
                  │
                  ▼
           Vercel (Angular)
                  │
                  ▼
       Render (ASP.NET Core API)
                  │
                  ▼
        Azure SQL Database
                  │
                  ▼
           Cloudinary Images

---

## Project Roadmap

```
Phase 1 ✅  Core Platform
            JWT auth · CRUD · Role guards · HttpOnly cookies · Image upload

Phase 2 ✅  Enterprise Upgrades
            Refresh token rotation · Pagination · Exception middleware
            API response wrapper · Serilog · Rate limiting · Config cleanup

Phase 3 🔄  Microservices Architecture
            YARP API Gateway · AuthService · ContentService · MediaService
            Separate DB per service

Phase 4 ⏳  Dockerization
            Dockerfile per service · docker-compose · containerized SQL Server

Phase 5 ⏳  AI Integration
            Article summary generator · Auto tag suggestion · Smart search
            LLM API integration · AI-powered content recommendations
```

---

## Tech Stack

**Frontend**
- Angular 20 with standalone components
- TypeScript · Reactive Forms · Angular Signals
- `httpResource()` for reactive HTTP
- `ngx-markdown` · `ng-select`
- Bootstrap 5

**Backend**
- ASP.NET Core 8 Web API
- Entity Framework Core 8
- ASP.NET Core Identity
- Serilog
- .NET 8 built-in Rate Limiter

**Database**
- Microsoft SQL Server
- Two DbContexts (ApplicationDbContext + AuthDbContext)
- EF Core Migrations

**Security**
- JWT Bearer Authentication
- HttpOnly + Secure + SameSite=Lax cookies
- Refresh Token Rotation
- BCrypt password hashing (via Identity)
- CORS with credential support

- ## ☁️ Deployment

| Layer | Platform |
|--------|----------|
| Frontend | Vercel |
| Backend API | Render |
| Database | Azure SQL Database |
| Image Storage | Cloudinary |

This production deployment demonstrates hosting a modern full-stack application using separate cloud services for each layer.

---

## Getting Started

### Prerequisites

```bash
Node.js 20+
.NET SDK 8.0
SQL Server (local or Docker)
Angular CLI 20+
```

### Backend Setup

```bash
# Clone the repository
git clone https://github.com/Gaurav-Vennamwar/secure-knowledge-management-systemv1.git
cd secure-knowledge-management-systemv1/SecureKnowledgeManagement.api

# Set up your dev config (not committed to git)
# Create appsettings.Development.json with:
{
  "ConnectionStrings": {
    "SKMSConnection": "Server=YOUR_SERVER; Database=SKMS_KnowledgeDB; Trusted_Connection=true; TrustServerCertificate=true"
  },
  "Jwt": {
    "Key": "your-secret-key-minimum-32-characters",
    "Issuer": "http://localhost:5251",
    "Audience": "http://localhost:4200"
  }
}

# Run migrations (creates all tables + seeds roles + admin user)
dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context AuthDbContext

# Start the API
dotnet run
# API runs at http://localhost:5251
# Swagger UI at http://localhost:5251/swagger
```

### Frontend Setup

```bash
cd SecureKnowledgeManagement.ui

# Install dependencies
npm install

# Start dev server
ng serve
# App runs at http://localhost:4200
```

### Default Admin Credentials

```
Email:    adminSKMS@gmail.com
Password: Admin123
Role:     Reader + Writer (full access)
```

> Register new users via `/register` — they receive the `Reader` role by default.

---

## API Reference

Base URL: `http://localhost:5251/api`

### Auth Endpoints
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/auth/register` | Register new user | Public |
| POST | `/auth/login` | Login · sets JWT + refresh token cookies | Public |
| POST | `/auth/logout` | Logout · revokes refresh token · clears cookies | Public |
| POST | `/auth/refresh` | Rotate refresh token · issue new JWT | Public |
| GET | `/auth/me` | Get current user from JWT claims | Bearer |

### Blog Post Endpoints
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/blogpost?pageNumber=1&pageSize=10` | Paginated blog posts | Public |
| GET | `/blogpost/{id:guid}` | Get by ID | Public |
| GET | `/blogpost/{urlHandle}` | Get by URL handle | Public |
| POST | `/blogpost` | Create blog post | Writer |
| PUT | `/blogpost/{id}` | Update blog post | Writer |
| DELETE | `/blogpost/{id}` | Delete blog post | Writer |

### Category Endpoints
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/categories` | Get all categories | Public |
| GET | `/categories/{id}` | Get by ID | Public |
| POST | `/categories` | Create category | Writer |
| PUT | `/categories/{id}` | Update category | Writer |
| DELETE | `/categories/{id}` | Delete category | Writer |

### Image Endpoints
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/images` | Upload image | Writer |
| GET | `/images` | List all images | Writer |

All responses follow the `ApiResponse<T>` wrapper:

```json
{
  "Success": true,
  "Message": "Blog posts fetched successfully",
  "Data": { ... },
  "StatusCode": 200
}
```

---

## Authentication Flow

```
LOGIN
──────
POST /auth/login
→ Validates email + password via ASP.NET Identity
→ Fetches user roles
→ Generates JWT (15 min expiry, HS256 signed)
→ Generates Refresh Token (cryptographically random, 7-day expiry)
→ Saves Refresh Token to RefreshTokens table
→ Sets access_token cookie  (HttpOnly · Secure · SameSite=Lax · 15min)
→ Sets refresh_token cookie (HttpOnly · Secure · SameSite=Lax · 7 days)
→ Returns { Email, Roles } (no token in response body)

SILENT REFRESH (Angular HTTP Interceptor)
──────────────────────────────────────────
JWT expires → any API call returns 401
→ Interceptor catches 401
→ Calls POST /auth/refresh with refresh_token cookie
→ Backend validates refresh token in DB (not expired, not revoked)
→ Revokes old refresh token (rotation)
→ Issues new JWT + new Refresh Token
→ Sets new cookies
→ Interceptor retries original request
→ User never sees a login prompt

LOGOUT
───────
POST /auth/logout
→ Reads refresh_token cookie
→ Marks token IsRevoked = true in DB
→ Overwrites both cookies with expired dates
→ User signal set to null → UI updates instantly
```

---

## Project Structure

```
secure-knowledge-management-systemv1/
├── SecureKnowledgeManagement.api/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── BlogPostsController.cs
│   │   ├── CategoriesController.cs
│   │   └── ImagesController.cs
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── AuthDbContext.cs          # Identity + RefreshTokens
│   ├── Middlewares/
│   │   └── GlobalExceptionMiddleware.cs
│   ├── Models/
│   │   ├── Domain/                   # EF Core entities
│   │   ├── DTO/                      # Request/Response DTOs
│   │   └── Wrappers/
│   │       └── ApiResponse.cs        # Generic response wrapper
│   ├── Repositories/
│   │   ├── Interface/                # Contracts
│   │   └── Implementation/           # EF Core implementations
│   └── Program.cs
│
└── SecureKnowledgeManagement.ui/
    └── src/
        └── app/
            ├── Core/
            │   ├── Components/
            │   │   └── navbar/
            │   ├── Interceptors/
            │   │   └── auth-interceptor.ts   # Auto JWT refresh
            │   └── Models/
            │       └── api-response.model.ts
            ├── Features/
            │   ├── Auth/
            │   │   ├── guards/
            │   │   │   └── admin-guard.ts
            │   │   ├── models/
            │   │   ├── services/
            │   │   │   └── auth-service.ts
            │   │   ├── login/
            │   │   └── register/
            │   ├── BlogPosts/
            │   │   ├── Models/
            │   │   ├── Services/
            │   │   ├── add-blogpost/
            │   │   ├── edit-blogpost/
            │   │   └── blogpost-list/
            │   ├── Category/
            │   └── Public/
            │       ├── home/
            │       └── blog-details/
            └── Shared/
                └── Components/
                    └── image-selector/
```

---

## Security Design Decisions

**Why HttpOnly cookies instead of localStorage?**
localStorage is accessible by JavaScript, making tokens vulnerable to XSS attacks. HttpOnly cookies are completely inaccessible to JavaScript — only the browser sends them automatically on requests. Combined with `Secure` and `SameSite=Lax` flags, this is the recommended approach for production applications.

**Why Refresh Token Rotation?**
A single long-lived refresh token that never changes is a security liability. With rotation, each use of the refresh token invalidates the old one and issues a new one. If a token is stolen and used by an attacker, the legitimate user's next request will fail (their token was already rotated), making theft detectable and limiting the damage window.

**Why two DbContexts?**
Separation of concerns — `ApplicationDbContext` manages business data (blogs, categories, images) while `AuthDbContext` manages identity data (users, roles, refresh tokens). This makes it straightforward to extract authentication into a separate microservice in Phase 3.

**Why Global Exception Middleware?**
Without it, unhandled exceptions expose stack traces and internal implementation details to clients. The middleware catches everything, logs the full error server-side via Serilog, and returns a clean, consistent `ApiResponse` to the client with no sensitive information.

---

## Developer

**Gaurav Vennamwar**
B.Tech Computer Science Engineering (AI/ML Specialization)

Building toward: Full-Stack → Cloud → AI/ML Engineering

[![GitHub](https://img.shields.io/badge/GitHub-Gaurav--Vennamwar-181717?style=flat-square&logo=github)](https://github.com/Gaurav-Vennamwar)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-gaurav--vennamwar-0A66C2?style=flat-square&logo=linkedin)](https://www.linkedin.com/in/gaurav-vennamwar-0b79b0212)
[![Email](https://img.shields.io/badge/Email-vennamwarg@gmail.com-D14836?style=flat-square&logo=gmail)](mailto:vennamwarg@gmail.com)
Portfolio
https://gaurav-portfolio-woad.vercel.app/

---

<div align="center">

**If this project helped you or impressed you, drop a ⭐ — it genuinely helps.**

*Built with intent. Shipped with care.*
*Designed, Developed and Deployed by.*

Gaurav Vennamwar

Full Stack .NET Developer

Always learning.
Always building.
Always shipping.

</div>
