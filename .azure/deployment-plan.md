# FluentDemo — Azure Deployment Plan

**Status:** Draft — awaiting user approval

## 1. Mode
**MODIFY** an existing .NET 10 solution to add Azure deployment artifacts. No application code changes required beyond config (BaseUrl override via App Settings).

## 2. Requirements
- **Goal:** Publish the FluentDemo Blazor Server app and its Minimal API to a permanent public URL for demo/sharing.
- **Audience:** Public, unauthenticated demo (no real data, no secrets).
- **Scale:** Low traffic; cheapest tier acceptable. Wake-on-request OK.
- **Region:** West Europe (closest to user). Confirm with user.
- **Budget:** Hobby/demo — keep ≤ ~$15/mo. Free F1 tier preferred for both apps if possible.

## 3. Components (from codebase scan)
| Component | Tech | Notes |
|---|---|---|
| `src/FluentDemo.Api` | ASP.NET Core 10 Minimal API | In-memory data; HTTP only on port 5062 locally. |
| `src/FluentDemo.Web` | Blazor Server (.NET 10) + FluentUI Blazor | Calls API via `HttpClient` named `Api` using `Api:BaseUrl` config. Needs **WebSockets** (Blazor Server uses SignalR). |
| `src/FluentDemo.Shared` | Class library | Shared records. |

## 4. Recipe
**AZD (Azure Developer CLI) + Bicep** — single `azd up` deploys both apps. Repeatable, infra-as-code, easiest for demos.

## 5. Target Architecture
- **Resource Group:** `rg-fluentdemo-<env>`
- **App Service Plan:** Linux, **F1 (Free)** — $0/mo. Limits: 60 CPU min/day, no Always On (cold start on first hit). Acceptable for demo. WebSockets are supported on Linux F1 for Blazor Server.
- **App Service — API:** `app-fluentdemo-api-<token>` running .NET 10. CORS: allow Web app origin. HTTPS only.
- **App Service — Web:** `app-fluentdemo-web-<token>` running .NET 10. WebSockets enabled. App setting `Api__BaseUrl` = `https://<api>.azurewebsites.net`. HTTPS only.
- **Identity:** System-assigned managed identity on both (not strictly required for demo — no DB/Storage — but enabled for future-proofing).
- **Telemetry:** _Skipped for cost._ Can add Application Insights later.

## 6. Artifacts to Create
- `azure.yaml` — AZD service manifest (api, web)
- `infra/main.bicep` — orchestration
- `infra/main.parameters.json`
- `infra/modules/appservice-plan.bicep`
- `infra/modules/appservice-api.bicep`
- `infra/modules/appservice-web.bicep`
- `.azure/deployment-plan.md` (this file)

## 7. Deployment Flow
1. `azd init` (already-prepared mode)
2. `azd auth login`
3. `azd up` — selects subscription/region, provisions plan + 2 App Services, builds and pushes both .NET projects.
4. Output public URL of Web app.

## 8. Cost Estimate
| Item | Tier | ~Monthly |
|---|---|---|
| App Service Plan (Linux F1) | F1 Free | **$0** |
| App Services | (free, included in plan) | $0 |
| Egress | trivial | ~$0 |
| **Total** | | **$0/mo** |

F1 limits: 60 CPU min/day, 1 GB RAM, no Always On, 165 MB/day outbound. Suitable for demo. Stop with `azd down` to remove all resources.

## 9. Security / Caveats
- App is intentionally **unauthenticated demo**. No production data.
- HTTPS only, TLS 1.2 min, FTP disabled.
- API CORS restricted to the Web app origin (avoid open `*`).

---

**Next:** Await user approval, then execute Phase 2 (generate artifacts, then hand off to azure-validate).
