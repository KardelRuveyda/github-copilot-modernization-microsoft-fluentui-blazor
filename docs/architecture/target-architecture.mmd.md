# 🏗️ Target Architecture (Mermaid)

> Code-as-source-of-truth version of the target architecture diagram.
> Keep this file in Git; renders automatically on GitHub.

---

## Runtime data flow

```mermaid
flowchart LR
    User((End User))

    subgraph Edge["Lane 1 — Edge & Identity"]
        FD["🌐 Azure Front Door<br/>TLS • CDN • Custom Domain"]
        WAF["🛡️ WAF Policy<br/>OWASP rules"]
        Idp["🔐 Identity Provider<br/>OIDC • MFA"]
    end

    subgraph App["Lane 2 — Application + Data (Azure App Service)"]
        YARP["🔁 YARP Reverse Proxy<br/>Strangler Fig"]
        Web["💻 Blazor Server (.NET 10)<br/>Microsoft FluentUI Blazor"]
        Api["⚙️ ASP.NET Core Web API<br/>.NET 10 • Dapper"]
        PE[("🔒 Private Endpoint")]
        SQL[("🗄️ Azure SQL<br/>Serverless • auto-pause")]
        KV["🔑 Azure Key Vault<br/>Managed Identity"]
        CICD["🚀 GitHub Actions<br/>CI/CD • Slot Swap"]
    end

    subgraph Obs["Lane 3 — Monitoring & Security"]
        AppI["📊 Application Insights"]
        Mon["📈 Azure Monitor"]
        LAW["🗂️ Log Analytics"]
        Def["🛡️ Defender for Cloud"]
        Cost["💰 Cost Management"]
    end

    %% Main business flow
    User -->|1| FD
    FD --- WAF
    FD -->|2| YARP
    YARP -->|3| Web
    Web -->|4| Api
    Api -->|5| PE --> SQL

    %% Auth flow
    Web -.OIDC.-> Idp
    Api -.JWT validate.-> Idp

    %% Secrets (Managed Identity)
    Web -.MI.-> KV
    Api -.MI.-> KV

    %% CI/CD
    CICD -.deploy.-> Web
    CICD -.deploy.-> Api

    %% Observability supporting flow
    Web -.logs/metrics.-> AppI
    Api -.logs/metrics.-> AppI
    AppI --- LAW
    Mon --- LAW
    Def --- LAW
    Cost --- LAW
```

---

## Future ideas (out of scope)

```mermaid
flowchart TB
    Cache["Azure Cache for Redis<br/>(read-side cache)"]
    Bus["Service Bus + Functions<br/>(async background workers)"]
    FF["Azure App Configuration<br/>(feature flags)"]
    E2E["Playwright E2E tests"]
    Micro["Microservice decomposition<br/>(if scale requires)"]
```

---

## Ownership legend

```mermaid
flowchart LR
    Infra["🟧 Infra owner<br/>Azure resources"]
    Devs["🟦 App owner<br/>Code, business logic"]
    Arch["🟪 Architecture<br/>Guidance, patterns"]
    Joint["🟨 Joint<br/>Cross-cutting work"]
```

---

_A reusable architecture diagram template, kept generic._
