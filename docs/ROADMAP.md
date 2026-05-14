# 🗺️ Modernization Roadmap

> End-to-end modernization plan for a legacy line-of-business web application.
> Reuse as a reference whenever an old UI (Web Forms / older component libraries) must be brought to modern Blazor on Azure, with hybrid connectivity to existing on-prem databases.

Current state (✅ done in this repo):
- **Blazor Server (.NET 10) + Microsoft FluentUI Blazor** UI
- **Minimal API** with mock data and mock login (placeholder)
- **Shared DTO library**
- **Azure App Service** deployment via **Bicep + `azd`**
- **GitHub Actions** CI/CD with **OIDC + Managed Identity**
- Live public demo URL

---

## 🧭 Guiding principle

Use a **hybrid order**:

1. First, **stop the bleeding** (Sprint -1): get the legacy app off the unprotected internet via a temporary lift-and-shift.
2. Then, lay the **infra foundations** (Sprint 0): network, secrets, telemetry, hybrid connectivity.
3. Then, modernize **vertically slice by slice** (data + strangler fig → auth → edge → ops), so every sprint produces a demoable change.

---

## 🏢 Prerequisite — Azure Landing Zone readiness

> Before any sprint starts, make sure the workload has a **landing zone** to live in.
> A landing zone is the pre-built Azure environment (subscription + policies + RBAC + central networking + central logging + identity baseline) that your workload **deploys into** — you don't build one per app, you join one.

What "ready" means:

- ✅ **Subscription provisioned** by the platform team (or sandbox for the demo)
- ✅ **Region(s) allowed** by Azure Policy (e.g. `West Europe`)
- ✅ **Required tags** known (`env`, `owner`, `cost-center`)
- ✅ **Central networking** decision made (hub-spoke vs. standalone; DNS strategy; firewall)
- ✅ **Central Log Analytics workspace** (or app-local LAW if no platform LAW exists)
- ✅ **Entra ID groups** for app team RBAC
- ✅ **Break-glass account** documented at the platform level

📚 References:
- [CAF — Azure Landing Zones](https://learn.microsoft.com/azure/cloud-adoption-framework/ready/landing-zone/)
- [Enterprise-Scale (ALZ Accelerator)](https://github.com/Azure/Enterprise-Scale)

> 💡 For this demo, the landing zone is intentionally minimal: a single subscription with default policies. In a real customer engagement, the platform team would deliver a richer one before Sprint -1 starts.

---

## 🚑 Sprint -1 — Emergency lift-and-shift ✅ (done before modernization starts)

> Goal: take the legacy app off the unprotected internet **immediately**, even before any modernization work.

| # | Task | Issue title | Status |
|---|------|-------------|--------|
| 0 | Lift-and-shift legacy app to **Azure App Service Managed Instance** (.NET FW 4.8, COM/registry/IIS, zero code changes) — eliminate direct internet exposure of on-prem servers | `chore(phase0): emergency lift-and-shift to App Service Managed Instance` | ✅ done — superseded by Phase 1 (Blazor .NET 10 + FluentUI) |

✅ Definition of done:
- Legacy app served from Azure (not from on-prem internet-facing server)
- Customer provides app files; infra coordinated centrally
- This phase is **temporary** — it will be retired once Phase 1 is complete

---

## 🏗️ Sprint 0 — Infra foundations + hybrid connectivity

> Goal: prepare the ground. The app keeps running, but the platform is production-aligned and connected to on-prem.

| # | Task | Issue title |
|---|------|-------------|
| 1 | Provision **VNet** + subnets (`app`, `data`, `func`, `gateway`) + **NSG** + **Private DNS Zones** | `feat(infra): add VNet with app/data/func/gateway subnets + NSG + Private DNS` |
| 2 | Provision **Log Analytics Workspace** + **Application Insights** + diagnostic settings | `feat(observability): add Log Analytics + Application Insights` |
| 3 | Provision **Azure Key Vault** + enable **System-assigned Managed Identity** on App Service + RBAC | `feat(security): add Key Vault and Managed Identity for App Service` |
| 4 | Provision **VPN Gateway (S2S, IPsec, VpnGw1)** + Local Network Gateway + Connection to on-prem network | `feat(infra): provision S2S VPN Gateway to on-prem network` |

✅ Definition of done:
- VNet + 4 subnets deployed; app keeps working.
- App Insights collects telemetry from Web + API.
- App Service has a System-assigned MI with `Key Vault Secrets User` role.
- VPN tunnel is **Connected**; a test VM in the `gateway` subnet can reach an on-prem IP.

---

## 🔌 Sprint 1 — Data + strangler fig + scheduled jobs

> Goal: replace mock data with a real database, put a reverse proxy in front (so legacy paths can keep working while migration continues), and stand up scheduled jobs that read from on-prem.

> 💡 **Why YARP this early:** the strangler fig pattern *is* the modernization. While the UI migration is in progress, some pages may still need to be served from the legacy system. A reverse proxy in Sprint 1 lets us route `/legacy/*` to the old app and `/new/*` to Blazor on the same domain.

| # | Task | Issue title |
|---|------|-------------|
| 5 | Provision **Azure SQL Database (Serverless, auto-pause)** + firewall closed + **Private Endpoint** in `data` subnet | `feat(infra): provision Azure SQL Serverless with Private Endpoint` |
| 6 | Enable **App Service VNet Integration** on `app` subnet | `feat(infra): enable App Service VNet integration` |
| 7 | Replace in-memory API with **ASP.NET Core Web API + Dapper** | `feat(api): replace in-memory store with ASP.NET Core Web API + Dapper` |
| 8 | Add **initial schema + seed scripts** | `feat(data): add initial database schema and seed scripts` |
| 9 | Add **YARP reverse proxy** (strangler fig): route `/api/*` → new API, `/legacy/*` → legacy origin, default → Blazor | `feat(gateway): introduce YARP reverse proxy (strangler fig pattern)` |
| 10 | Add **Azure Functions** project (Timer-triggered, e.g. monthly job on day 5) with VNet integration + access to on-prem SQL via VPN | `feat(functions): scheduled monthly job (Timer-triggered) reading on-prem data via VPN` |

✅ Definition of done:
- API reads/writes Azure SQL through Private Endpoint.
- Connection strings in Key Vault, fetched via Managed Identity.
- YARP routes traffic correctly: new pages → Blazor, legacy pages → legacy origin, both behind one URL.
- Monthly Timer Function runs in Azure and can reach on-prem SQL via VPN (idempotent writes).

---

## 🔐 Sprint 2 — Authentication and identity

> Goal: remove mock login. Use a real identity provider with MFA.

| # | Task | Issue title |
|---|------|-------------|
| 11 | Set up **Microsoft Entra External ID** tenant + app registration | `feat(auth): set up Entra External ID tenant and app registration` |
| 12 | Add `Microsoft.Identity.Web` to Blazor; replace mock login with OIDC sign-in (MFA + Conditional Access) | `feat(auth): replace mock login with Entra External ID (OIDC + MFA)` |
| 13 | Protect API with **JWT Bearer**; pass tokens from Web → API | `feat(auth): protect API with JWT Bearer and downstream token flow` |

✅ Definition of done:
- Users sign in with Microsoft account.
- MFA enforced via Conditional Access.
- API rejects unauthenticated calls.

---

## 🌐 Sprint 3 — Managed edge

> Goal: put a managed edge in front of everything (custom domain, TLS, WAF).

| # | Task | Issue title |
|---|------|-------------|
| 14 | Provision **Azure Front Door** + **WAF policy** (OWASP rules) + custom domain + TLS | `feat(infra): add Azure Front Door + WAF in front of the app` |
| 15 | Restrict App Service ingress to Front Door only (service tag + `X-Azure-FDID`) | `feat(security): restrict App Service ingress to Front Door` |

✅ Definition of done:
- Public traffic enters via Front Door → WAF → App Service.
- Direct App Service URL returns 403.

---

## 🚀 Sprint 4 — Operations, security, FinOps

> Goal: production-grade deployment, monitoring, and cost guardrails.

| # | Task | Issue title |
|---|------|-------------|
| 16 | Add **staging slot** + GitHub Actions **slot swap** with smoke tests | `ci: add staging slot + slot swap deployment` |
| 17 | Run **`bicep what-if`** on PRs | `ci: run bicep what-if on pull requests` |
| 18 | Enable **Microsoft Defender for Cloud** plans for App Service, SQL, Key Vault, Storage, DNS and Resource Manager | `feat(security): enable Defender for Cloud plans (App Service, SQL, KV, Storage, DNS, ARM)` |
| 19 | Add **Cost Management** budgets + alerts + tagging policy | `feat(finops): add budgets, alerts and tagging policy` |
| 20 | Add **Azure Monitor alerts** (availability, 5xx, CPU, DTU) and dashboards | `feat(observability): add Azure Monitor alerts and dashboards` |

✅ Definition of done:
- Zero-downtime deploys via slot swap.
- PRs show infra diff before merge.
- Alerts page the on-call team.
- Monthly cost budget with email alerts.

---

## 🔭 Future ideas (out of scope for now)

- Retire the temporary lift-and-shift Managed Instance (Phase 0)
- Decompose into **microservices** if growth justifies it
- Add **background workers** for async processing (Service Bus + Functions)
- Add a **read-side cache** (Azure Cache for Redis)
- Add **end-to-end tests** with Playwright
- Add **feature flags** (Azure App Configuration)
- Stored procedure refactoring / ETL with Azure Data Factory

---

## 📋 Suggested issue creation order

```
Sprint -1 (already done — open and immediately close)
  0. chore(phase0): emergency lift-and-shift to App Service Managed Instance

Sprint 0
  1. feat(infra): VNet + subnets + NSG + Private DNS
  2. feat(observability): Log Analytics + Application Insights
  3. feat(security): Key Vault + Managed Identity
  4. feat(infra): S2S VPN Gateway to on-prem network

Sprint 1
  5. feat(infra): Azure SQL Serverless + Private Endpoint
  6. feat(infra): App Service VNet integration
  7. feat(api): ASP.NET Core Web API + Dapper
  8. feat(data): initial schema + seed scripts
  9. feat(gateway): YARP reverse proxy (strangler fig)
 10. feat(functions): monthly Timer-triggered job via VPN

Sprint 2
 11. feat(auth): Entra External ID tenant + app registration
 12. feat(auth): Blazor OIDC sign-in (MFA + CA)
 13. feat(auth): API JWT Bearer + downstream tokens

Sprint 3
 14. feat(infra): Front Door + WAF
 15. feat(security): restrict App Service ingress to Front Door

Sprint 4
 16. ci: staging slot + slot swap
 17. ci: bicep what-if on PR
 18. feat(security): Defender for Cloud plans (App Service, SQL, KV, Storage, DNS, ARM)
 19. feat(finops): budgets + alerts + tagging
 20. feat(observability): Azure Monitor alerts and dashboards
```

---

_A reference modernization plan — covers emergency lift-and-shift, foundational infra (incl. hybrid VPN), strangler-fig data/UI migration, identity, edge protection, and operational hardening._
