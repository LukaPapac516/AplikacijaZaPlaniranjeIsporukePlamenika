---
name: edit-form
description: "Use when: creating or improving MVC create/edit forms with validation, sectioned fields, and save/cancel flow."
---

# Edit Form Skill

## Purpose
Create robust edit/create forms for business entities with clear validation behavior.

## Scope
- `Controllers/*Controller.cs` (GET/POST Create/Edit)
- `Views/<Entity>/Create.cshtml` and/or `Views/<Entity>/Edit.cshtml`
- Validation partial usage (`_ValidationScriptsPartial`)

## UX baseline
- Breadcrumb and page title.
- Validation summary + inline field errors.
- Grouped inputs in logical sections.
- Primary save and secondary cancel/back actions.

## Workflow
1. Add GET action to load entity and required dropdown lists.
2. Add POST action with ModelState and business-rule checks.
3. Keep user input when validation fails.
4. Render field-level messages (`asp-validation-for`).
5. Include scripts partial for client validation.
6. Run build and test valid/invalid submit flow.

## Guardrails
- Never silently drop user changes on validation failure.
- Always validate date/order business rules explicitly.
- For dropdown FK fields, re-populate options when returning invalid model.

## Example prompts
- "Napravi Edit formu za RadniNalog s validacijom datuma zatvaranja."
- "Dodaj Create formu za FazaProjekta s validacijom redoslijeda."
