# List Page Skill — Example

Purpose: show a concrete example of creating a list/index page with filter, KPI chips and table.

Example: `RadniNalozi` index page (already implemented in this repo).

Steps performed in this example:
1. Controller: `Controllers/RadniNaloziController.cs` — `Index(string? search, StatusNaloga? status)` implements filtering, `Include` for related `Projekt`, ordering and KPI counts.
2. View: `Views/RadniNalozi/Index.cshtml` — breadcrumb, header, KPI chips, filter row (search + status), table with actions and empty state.
3. Navigation: add menu link in `Views/Shared/_Layout.cshtml` to the controller's `Index` action.

Verification:
```powershell
dotnet build
# run app and open /radni-nalozi or /RadniNalozi/Index in browser
```

If you need to create a new list page from scratch, follow the workflow in `SKILL.md` and use this file as a template: implement controller Index, include navigation properties with `Include`, then add view with KPI/filter/table.
