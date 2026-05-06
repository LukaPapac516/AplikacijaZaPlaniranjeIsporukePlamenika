# Entity Framework Skill — Example

Purpose: demonstrate the typical workflow for making an EF model change, generating a migration and applying it.

Scenario: add a new nullable column `ExternalReference` (string, max 100) to `Project`.

Steps:
1. Edit `Models/Project.cs` — add the new property with DataAnnotation:

   - `public string? ExternalReference { get; set; }`

2. Update `Data/AppDbContext.cs` only if you need fluent configuration. For this change no extra fluent mapping is required.

3. Build the project and fix compile issues:

```powershell
dotnet build
```

4. Create migration (example name):

```powershell
dotnet ef migrations add Add_Project_ExternalReference
```

5. Apply migration to database:

```powershell
dotnet ef database update
```

6. Smoke test app (run and navigate to Projects list/details).

Notes:
- Follow guardrails in `SKILL.md`: keep delete behavior explicit and run `dotnet build` before adding migration.
- If migration modifies FKs or delete behavior, inspect generated migration files before applying.

Example prompts:
- "Dodaj nullable string `ExternalReference` u `Project` i generiraj migraciju"
- "Promijeni onDelete za StavkaProizvodnje.RadniNalog na Restrict i generiraj migraciju"
