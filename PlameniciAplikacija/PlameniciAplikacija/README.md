# PlameniciAplikacija

ASP.NET Core MVC aplikacija za upravljanje projektima, kupcima i djelatnicima u planiranju isporuke plamenika.

## UX Sub-agent Evidence
- UX agent definition: `.github/agents/ux.agent.md`
- Main orchestration rules: `.github/copilot-instructions.md`
- Runtime/log proof (stored in repo): `docs/UX_SPAWN_LOG_EVIDENCE.md`

## UI Scope
- Unique command-center homepage: `Views/Home/Dashboard.cshtml`
- List + details flow with breadcrumbs:
  - Projekti (`Views/Home/Index.cshtml`, `Views/Home/Details.cshtml`)
  - Kupci (`Views/Kupci/Index.cshtml`, `Views/Kupci/Details.cshtml`)
  - Djelatnici (`Views/Djelatnici/Index.cshtml`, `Views/Djelatnici/Details.cshtml`)
