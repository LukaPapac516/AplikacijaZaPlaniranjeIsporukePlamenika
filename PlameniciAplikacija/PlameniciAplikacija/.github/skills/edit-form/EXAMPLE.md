# Edit Form Skill — Example

Purpose: concrete example to create/validate an edit form for `RadniNalog`.

Files in this repo used by the example:
- `Controllers/RadniNaloziController.cs` — `Edit(int id)` GET and `Edit(int id, RadniNalog updated)` POST with server-side date validation.
- `Views/RadniNalozi/Edit.cshtml` — breadcrumb, validation summary, field-level validation, project dropdown, Save/Cancel buttons and `_ValidationScriptsPartial`.

Workflow summary:
1. GET action loads entity and calls `PopulateProjects` for dropdown data.
2. POST action validates business rules (e.g. `DatumZatvaranja >= DatumOtvaranja`), adds `ModelState` errors when necessary and returns view with preserved user input.
3. On successful validation, save via `_context.SaveChanges()` and redirect to Index.

Test steps:
```powershell
dotnet build
# run app and navigate to /radni-nalozi, click Uredi on a row
# try invalid date (zatvaranja < otvaranja) and verify error shown
```

Use this example as a template for new create/edit forms described in `SKILL.md`.
