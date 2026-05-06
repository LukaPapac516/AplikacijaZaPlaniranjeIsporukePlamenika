---
name: list-page
description: "Use when: creating or refactoring MVC list/index pages with filters, table overview, status visibility, and navigation links."
---

# List Page Skill

## Purpose
Create a business-readable list page with consistent layout and quick scanning.

## Scope
- `Controllers/*Controller.cs` (Index action)
- `Views/<Entity>/Index.cshtml`
- Optional nav update in `Views/Shared/_Layout.cshtml`

## UX baseline
- Header with title + subtitle.
- Filter row (search and status/type filter when relevant).
- KPI chips for quick counts.
- Table with clear columns and row-level action.
- Breadcrumb and return path.

## Workflow
1. Add/extend `Index` action with filter parameters and ordering.
2. Include required navigation properties (`Include`) for table data.
3. Build `Index.cshtml` with breadcrumb, filters, KPI chips, and table.
4. Add links from row to details/edit action.
5. Validate empty-state message for no results.
6. Run build.

## Guardrails
- Keep one primary table per page.
- Use consistent classes already in project (`lc-header`, `lc-kpi-row`, `lc-table-shell`).
- Do not introduce unrelated UI frameworks.

## Example prompts
- "Napravi list stranicu za RadniNalog s pretragom i status filterom."
- "Refaktoriraj Kupci listu da ima KPI kartice i bolji pregled."
