# Pottery Service

Clean Architecture 4 layer with `.NET 6`.

## Structure

```text
src
|-- PotteryService.Domain
|   |-- Common
|   |-- Entities
|   `-- Enums
|-- PotteryService.Application
|   |-- Common
|   |-- Features
|   `-- DependencyInjection.cs
|-- PotteryService.Infrastructure
|   |-- Persistence
|   |-- Repositories
|   |-- Services
|   `-- DependencyInjection.cs
`-- PotteryService.Api
    |-- Controllers
    |-- Middleware
    `-- Program.cs
```

## Layers

- `Domain`: entity, enum, business rule core.
- `Application`: use case, DTO, interface/abstraction.
- `Infrastructure`: EF Core, repository, database wiring.
- `Api`: presentation layer, controller, middleware, DI bootstrap.

## Run

```bash
dotnet restore
dotnet build
dotnet run --project src/PotteryService.Api
```

Đây là base ban đầu, chưa có feature/domain sample.
