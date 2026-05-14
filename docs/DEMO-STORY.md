# FluentDemo – What I Built and How 🚀

> A short, friendly walkthrough of the FluentDemo project: a Blazor + FluentUI demo app, deployed live on Azure, with automatic updates on every push.

🔗 **Live demo:** https://app-fluentdemo-web-ygi6c27opusmo.azurewebsites.net/
📦 **GitHub repo:** https://github.com/KardelRuveyda/github-copilot-modernization-microsoft-fluentui-blazor

---

## 1. What is this project?

FluentDemo is a small but realistic demo app that shows how to build a modern web application with:

- **Blazor Server** on **.NET 10**
- **Microsoft FluentUI Blazor** components for a clean, Microsoft-style UI
- A **Minimal API** backend
- A **shared class library** for DTOs (data models used by both API and Web)

The goal is to use this as a teaching/demo project for **GitHub Copilot modernization** scenarios.

---

## 2. Project structure

```
FluentDemo.slnx
└── src/
    ├── FluentDemo.Api/      → Minimal API (login, incentives, users)
    ├── FluentDemo.Web/      → Blazor Server UI (FluentUI components)
    └── FluentDemo.Shared/   → Shared models (AppUser, Incentive, LoginRequest, ...)
```

- The **Web** project calls the **API** using a named `HttpClient` ("Api").
- The **Shared** project keeps DTOs in one place so both sides stay in sync.

---

## 3. What I did, step by step

### Step 1 – Cleanup and rename
The project was renamed and tidied up. I fixed:
- Solution name
- Folder names
- Project file names
- Namespaces
- UI texts and branding

### Step 2 – Make it run again
I rebuilt the API as a small in-memory service with these endpoints:
- `POST /api/auth/login`
- `GET  /api/incentives`
- `GET  /api/users`

### Step 3 – Polish the UI
The notification popover had too much empty space and titles were hidden.
I rewrote `NotifItem.razor` with a simple flex layout and added compact CSS in `app.css`.
Now notifications look clean and readable. ✨

### Step 4 – Publish to GitHub
- Created a public repo
- Added a clear README, description, and topics
- Added a Codespaces dev container so anyone can open it in the browser and run it

### Step 5 – Deploy to Azure (cheap!)
I wanted the cheapest option, so I used:
- **Azure App Service – Free tier (F1)** for both API and Web
- **Bicep** files in `infra/` to describe the infrastructure as code
- **Azure Developer CLI (`azd`)** to provision and deploy

Files added:
- `azure.yaml` – tells `azd` about the services
- `infra/main.bicep` – defines resource group, App Service plan, two web apps, CORS
- `.azure/deployment-plan.md` – the deployment plan document

### Step 6 – Automatic updates with GitHub Actions
I set up CI/CD so that **every push to `main` updates the live demo automatically**.

- Workflow file: `.github/workflows/azure-dev.yml`
- Authentication uses **OIDC + a User-Assigned Managed Identity** (no secrets stored in GitHub)
- Steps: checkout → setup `azd` → login → provision → package → deploy

### Step 7 – Fix the CI failure 🛠️
The first pipeline run failed with this error:

```
error MSB4018: The "GenerateDepsFile" task failed unexpectedly.
System.IO.IOException: The process cannot access the file
'.../FluentDemo.Shared/bin/Release/net10.0/FluentDemo.Shared.deps.json'
because it is being used by another process.
```

**Why?** `azd deploy` was packaging the API and the Web project **in parallel**.
Both projects use `FluentDemo.Shared`, so they both tried to write the same `.deps.json` file at the same time → file lock.

**Fix:** Package the services **sequentially** before deploying:

```yaml
- name: Package services (sequential to avoid Shared.dll lock)
  run: |
    azd package api --no-prompt
    azd package web --no-prompt

- name: Deploy Application
  run: azd deploy --no-prompt
```

After this fix, the pipeline turned green ✅ and the live URL now updates on every push.

---

## 4. How the deployment flow works

```
Developer pushes to main
        │
        ▼
GitHub Actions starts (azure-dev.yml)
        │
        ▼
azd login (OIDC, no passwords)
        │
        ▼
azd provision  →  Bicep creates/updates Azure resources
        │
        ▼
azd package api  →  dotnet publish for API
azd package web  →  dotnet publish for Web   (sequential!)
        │
        ▼
azd deploy  →  Uploads ZIPs to Azure App Service
        │
        ▼
Live demo is updated 🎉
```

---

## 5. Key things I learned

- **`azd`** makes Azure deployments very easy: one config file + Bicep + one command.
- **Parallel builds can fight** when projects share a library. Sequential packaging is a safe fix.
- **OIDC + Managed Identity** is the modern way to authenticate from GitHub Actions to Azure. No secrets to rotate.
- **App Service Free (F1)** is enough for a small demo and costs **$0**.
- **Codespaces + dev container** lets anyone try the project in their browser, with no local setup.

---

## 6. How to try it

### Option A – Just open the live demo
👉 https://app-fluentdemo-web-ygi6c27opusmo.azurewebsites.net/

### Option B – Open in GitHub Codespaces
1. Go to the repo
2. Click **Code → Codespaces → Create codespace on main**
3. Wait for the container to start, then run the app

### Option C – Run locally
```powershell
git clone https://github.com/KardelRuveyda/github-copilot-modernization-microsoft-fluentui-blazor.git
cd github-copilot-modernization-microsoft-fluentui-blazor
dotnet run --project src/FluentDemo.Api
dotnet run --project src/FluentDemo.Web
```

---

## 7. What's next?

Ideas for the next iterations:
- Add a real database (Azure SQL or Cosmos DB)
- Add authentication with Microsoft Entra ID
- Add Application Insights for telemetry
- Add unit tests and a test job in the pipeline
- Add a "Deploy status" badge in the README

---

_Made with ❤️, Blazor, FluentUI, Azure, and a lot of help from GitHub Copilot._
