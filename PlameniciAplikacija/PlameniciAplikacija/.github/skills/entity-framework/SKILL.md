---
name: entity-framework
description: "Use when: changing EF model classes, updating AppDbContext relations, generating migrations, or updating database schema."
---

# Entity Framework Skill

## Purpose
Use this skill for safe and consistent EF Core changes in this project.

## Scope
- `Models/*.cs`
- `Data/AppDbContext.cs`
- `Program.cs` (DI/DbContext registration)
- `Migrations/*`
- `appsettings.json` (connection string)

## Workflow
1. Inspect affected model classes and current relationships.
2. Apply model updates with DataAnnotations and navigation properties.
3. Update `AppDbContext` fluent mapping if relationship behavior changes.
4. Build solution and resolve compile issues.
5. Create migration (`dotnet ef migrations add <Name>`).
6. Apply migration (`dotnet ef database update`).
7. Verify relationship behavior (cascade/set null/restrict) in generated migration.

## Guardrails
- Preserve existing enum semantics and naming conventions.
- Do not remove/rename existing columns without explicit requirement.
- Keep delete behavior explicit in fluent config.
- After each schema change: run build and verify migration output.

## Example prompts
- "Dodaj novu FK vezu iz Napomena na Project i generiraj migraciju."
- "Promijeni onDelete behavior za StavkaProizvodnje -> RadniNalog na Restrict."
- "Dodaj validacijske anotacije na novi EF model i updateaj DbContext."
