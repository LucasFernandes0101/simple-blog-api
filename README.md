# Simple Blog API

> RESTful API for managing blog posts and comments — built with .NET 10 and a clean architecture style using a **Command / Use Case** pattern.
> Only environment variable required: `SQLSERVER_CONNECTION_STRING`.

---

## Table of Contents

* [Overview](#overview)
* [Architecture & Patterns](#architecture--patterns)
* [Key Libraries and Tools](#key-libraries-and-tools)
* [Project Structure](#project-structure)
* [API Endpoints](#api-endpoints)
* [Running the Application](#running-the-application)
* [Testing](#testing)
* [Best Practices & Notes](#best-practices--notes)
* [Roadmap / TODO](#roadmap--todo)
* [Contributing](#contributing)

---

## Overview

Simple Blog API provides endpoints to manage `BlogPost` and `Comment` entities. The project is designed to be maintainable, testable, and extensible.
The API automatically applies any necessary database migrations at startup.
**Swagger** is available for interactive API documentation and **Serilog** is used for structured logging. A custom exception middleware centralizes error handling and returns consistent error responses.
API routing is versioned and exposed under the `/api/v1` prefix.

---

## Architecture & Patterns

* **Command / Use Case Pattern** — each action is represented by a command (e.g., `CreateBlogPostCommand`) and executed by a dedicated handler that validates input, executes business logic, and persists data. Handlers are single-responsibility use cases.
* **Repository Pattern** — repository interfaces (`IBlogPostRepository`, `ICommentRepository`) abstract persistence so handlers remain testable and infrastructure-agnostic.
* **Validation with FluentValidation** — commands are validated before execution; invalid commands throw `ValidationException`.
* **Explicit Mapping** — entities are mapped to DTOs / result objects via mapper extensions (`ToCreateResult()`, `ToDetailResult()`), preventing domain leakage to API responses.
* **Custom Exception Middleware** — centralizes error handling, maps domain exceptions to proper HTTP status codes, and returns a consistent error payload format.
* **Structured Logging** — Serilog captures structured logs for requests, responses and exceptions to assist observability.
* **API Versioning** — the project uses ASP.NET API versioning (Asp.Versioning.Mvc) and exposes endpoints under `/api/v1`, allowing safe evolution of public contracts.

---

## Key Libraries and Tools

* **.NET 10**
* **MediatR** — command/handler orchestration
* **AutoMapper** — entity-to-DTO/result mapping
* **FluentValidation** — command validation
* **Entity Framework Core** — persistence and automatic migrations on startup
* **Asp.Versioning.Mvc** — API versioning (routes exposed under `/api/v1`)
* **Bogus** — fakers for test data
* **NSubstitute** — mocks for unit testing
* **xUnit** — test framework
* **FluentAssertions / Shouldly** — expressive assertions
* **Swagger / Swashbuckle** — interactive API documentation (`/swagger`)
* **Serilog** — structured logging
* **Custom Exception Middleware** — centralized error handling

---

## Project Structure

```
/src
  /SimpleBlogApi
    /Application
      /Commands        # Command objects representing actions
      /Handlers        # Use-case handlers executing commands
      /Mappers         # Entity -> Result mapping
      /Results         # Output DTOs and response shapes
      /Validators      # FluentValidation validators
    /Domain
      /Entities        # Domain entities: BlogPost, Comment (Comment includes Author)
      /Base            # BaseEntity (Id, CreatedAt)
      /Interfaces      # Repository interfaces
    /Infrastructure    # Concrete persistence implementations (EF Core), if present
    /Controllers       # API controllers exposing routes (versioned under /api/v1)
    /Middlewares       # Custom Exception Middleware and other middlewares
    Program.cs          # Application bootstrap, DI, middleware (Serilog, Swagger, ExceptionMiddleware)
 /tests
  /SimpleBlogApi.Tests # Unit tests (handlers, validators, mappers)
SimpleBlogApi.slnx
README.md
```

---

## API Endpoints

> Note: The API exposes versioned endpoints under `/api/v1`. The API design places the blog post identifier in the route for operations related to comments. The `Comment` payload includes an `author` field.

### Posts

* **List Posts (Paged)**

  ```
  GET /api/v1/posts?_page={page}&_size={size}
  ```

  Response example:

  ```json
  {
    "total": 25,
    "items": [
      {
        "id": 1,
        "title": "Hello World",
        "content": "This is my first post",
        "createdAt": "2025-11-20T22:00:00Z"
      }
    ]
  }
  ```

* **Get Post Details (with comments)**

  ```
  GET /api/v1/posts/{id}
  ```

  Response example:

  ```json
  {
    "id": 1,
    "title": "Hello World",
    "content": "This is my first post",
    "createdAt": "2025-11-20T22:00:00Z",
    "comments": [
      {
        "id": 1,
        "author": "Jane Doe",
        "content": "Great post!",
        "createdAt": "2025-11-20T22:05:00Z"
      }
    ]
  }
  ```

* **Create Post**

  ```
  POST /api/v1/posts
  ```

  Body example:

  ```json
  {
    "title": "New Post",
    "content": "This is a new blog post."
  }
  ```

### Comments

* **Create Comment under a BlogPost**

  ```
  POST /api/v1/posts/{postId}/comments
  ```

  Path parameter: `postId` — id of the blog post the comment belongs to.
  Body example:

  ```json
  {
    "author": "John Doe",
    "content": "Nice post!"
  }
  ```

---

## Running the Application

1. Set the `SQLSERVER_CONNECTION_STRING` environment variable:

**Windows (PowerShell)**

```powershell
$env:SQLSERVER_CONNECTION_STRING="Server=your-sql-host,1433;Database=SimpleBlogDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;Encrypt=False"
```

**Linux / macOS**

```bash
export SQLSERVER_CONNECTION_STRING="Server=your-sql-host,1433;Database=SimpleBlogDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;Encrypt=False"
```

2. Restore, build and run:

```bash
dotnet restore
dotnet build
dotnet run --project src/SimpleBlogApi
```

* Swagger UI will be available at `/swagger`.
* Serilog outputs structured logs according to configuration in `Program.cs`.
* The application automatically applies migrations at startup.

---

## Testing

* Unit tests use **xUnit**, **NSubstitute**, **FluentAssertions**, **Shouldly** and **Bogus**.
* Tests instantiate mocks per test — no global shared mocks.
* Tests cover handlers (use-cases), validations and mappers; all database access is mocked, avoiding in-memory DB usage for unit tests.
* Run tests:

```bash
dotnet test
```

---

## Best Practices & Notes

* Handlers are focused on orchestration: validate → call domain/repositories → map result. Keep domain rules in domain services or value objects when complexity grows.
* Centralize validation with FluentValidation for consistent error messages and to keep handlers clean.
* Explicit mapping prevents accidental exposure of domain internals to API consumers.
* The custom exception middleware ensures consistent HTTP status codes and response shapes for errors; keep it small, testable and extensible.
* Serilog provides structured logs — ensure sensitive information is excluded from logs.
* Unit tests should remain isolated; create fakers for repeatable test data and avoid shared mutable state.

### Contracts & Endpoint Segregation (Best Practice)

* **Per-endpoint contracts:** each endpoint defines its own request and response DTOs — fully segregated contracts per route and HTTP verb.
* **Conversion flow:** request DTOs are mapped to `Command` objects (input for handlers); handlers return `Result` objects which are then mapped to response DTOs sent to clients.
* **Benefits:** changes to one endpoint contract do not affect other endpoints or internal domain models; this increases safety, backwards compatibility, and API evolution.
* **Example flow:**

  ```
  Request DTO  →  CreateBlogPostCommand → Handler → CreateBlogPostResult → Response DTO
  ```
* **Implementation notes:** keep DTO-to-Command mappers and Result-to-DTO mappers colocated with controllers or in dedicated mapping classes, and avoid returning domain entities directly from controllers.

---

## Roadmap / TODO

* **Add user registration / user entity** — a user model, persistence, and user-related commands/handlers.
* **Add authentication & authorization** — JWT or another robust scheme, policies and role-based access for endpoints.
* **Add integration tests** — end-to-end tests that run against a test database to validate real persistence behavior and middleware (including exception middleware and Serilog integration).
* **Add Docker support** — `Dockerfile` + `docker-compose` (API + SQL Server) and CI pipelines that can run integration tests in containers.

---

## Contributing

* Fork the repository.
* Create a feature branch and implement your changes following the Command / Use Case pattern.
* Keep handlers small and testable; add validators and mappers as necessary.
* Ensure all unit tests pass: `dotnet test`.
* Open a pull request to the `develop` branch with a clear description of the changes.
