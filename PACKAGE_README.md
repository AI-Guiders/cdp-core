# AIGuiders.Cdp.Core

Shared **.NET 10** library for the Cognitive Dev Platform tool catalog:

`catalog = f(phase, object)` with optional `intent` ranking — used by **Cursor `cdp-mcp`** and **Cascade IDE** MCP ListTools filtering.

**License:** MIT  
**Source:** [github.com/AI-Guiders/cdp-core](https://github.com/AI-Guiders/cdp-core)

---

## Install

```bash
dotnet add package AIGuiders.Cdp.Core
```

Prefer a sibling `ProjectReference` to `cdp-core` in the monorepo when developing; NuGet for standalone clones / CI.

---

## Public API (short)

| Type | Role |
|------|------|
| `CdpPhase` / `CdpObjectKind` / `CdpIntent` | Finite catalog axes (not free-text goals) |
| `ToolAffordance` | Tool metadata: phases, objects, intents, cost/risk |
| `SessionContext` | Current phase×object(+intent) |
| `PhaseObjectCatalog.Query` | Filter + rank shortlist |
| `Wave1AffordanceSeed` | Seed table for CDP wave1 backends (optional host data) |
| `ICdpBackendModule` | Host backend contract (optional) |

Non-goal: free-text **goal** as a catalog key (stays in agent reasoning).

---

## Consumers

- `cdp-mcp` — stdio facade (meta `cdp_*` + shortlist)
- `CascadeIDE` — `ide_context` / `ide_tools` + filtered `ide_*` ListTools
