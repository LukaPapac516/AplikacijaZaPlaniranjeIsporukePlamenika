# Copilot Instructions

## Sub-agent orchestration for UI tasks

When the user asks for UI/UX code generation or edits (views, components, CSS, layout, forms, table screens):
1. Spawn the UX sub-agent first.
2. Use the UX output as constraints for implementation.
3. Then generate or modify UI code.
4. Summarize which UX rules were applied.

Preferred spawn call:
- tool: runSubagent
- agentName: UX
- prompt source: .github/agents/ux.agent.md

Fallback if UX agent is unavailable in the active session:
- Use runSubagent with agentName Explore and pass the same UX rules from .github/agents/ux.agent.md.

## Mandatory UX rules
- Clean business UI
- Clear project table overview
- Clear status indicators
- Mark priority projects
- Simple navigation
- Consistent layout
- Form validation
- Minimal design
- Focus on business users
