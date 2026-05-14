# FluentUI Blazor Demo

[![Live Demo](https://img.shields.io/badge/Live%20Demo-Azure%20App%20Service-0078D4?logo=microsoftazure&logoColor=white)](https://app-fluentdemo-web-ygi6c27opusmo.azurewebsites.net/)
[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/KardelRuveyda/github-copilot-modernization-microsoft-fluentui-blazor?quickstart=1)

🌐 **Live demo:** <https://app-fluentdemo-web-ygi6c27opusmo.azurewebsites.net/>
Sign in with username `admin` and any non-empty password.

A showcase application built with **.NET 10**, **Blazor Server** and **Microsoft FluentUI Blazor**.
The goal of this repository is to demonstrate how the FluentUI Blazor component library can be
used to build a rich, Azure Portal-style web application — including data grids, charts,
dialogs, navigation, theming and localization — on top of the latest .NET stack.

> No real backend, no real authentication. Everything runs in-memory so the demo can be
> cloned and started in a few seconds.

---

## ✨ What this demo shows

This is a small, opinionated tour of the FluentUI Blazor component library. The pages in
this app are intentionally varied so that each one focuses on a different set of components
and patterns.

| Area | FluentUI components / patterns used |
| --- | --- |
| **App shell**         | Custom native HTML shell with `FluentPopover`, `FluentButton`, `FluentIcon`, `FluentSearch`, `FluentDesignTheme`, `FluentToastProvider`, `FluentDialogProvider`, `FluentMenuProvider`, `FluentMessageBarProvider`, `FluentTooltipProvider` |
| **Navigation**        | `FluentNavMenu`, `FluentNavLink`, `FluentNavGroup`, custom collapsed rail, command-palette style search dropdown |
| **Login**             | Custom themed login screen, native inputs, `FluentMessageBar`, `FluentButton` with `Loading` state |
| **Dashboard / Home**  | KPI cards, command bar, service tiles, activity timeline (CSS grid + FluentUI icons) |
| **Incentives list**   | `FluentDataGrid` with `PropertyColumn`, `TemplateColumn`, sorting, filter chips, three-dot row menu, pagination, status badges |
| **Create wizard**     | Multi-step form with `FluentSelect`, `FluentTextArea`, `FluentRadioGroup`, `FluentSwitch`, `FluentNumberField`, `FluentDatePicker` |
| **Reports & analytics** | KPI cards, **SVG pie / donut / area / line / gauge charts**, **activity heatmap**, leaderboard data grid, Excel export |
| **Users CRUD**        | Toolbar, `FluentDataGrid` with pagination, `FluentDialog` for create/edit, `FluentSelect`, `FluentSwitch`, `IToastService` |
| **Resources**         | Split panel with `FluentTreeView` and a detail pane |
| **Activity log**      | Filterable `FluentDataGrid` with level badges, `FluentPersona`, pagination, Excel export |
| **Settings**          | `FluentTabs` with five tabs, theme + accent color pickers, security list, system info |
| **Chat assistant**    | Floating FAB built from FluentUI primitives, button-driven (no AI), keyword routing |
| **Theming**           | `FluentDesignTheme` with light / dark / system modes and customizable primary color |
| **Localization**      | Custom `LocalizationService` with EN / TR strings, live switching via the language popover |
| **Excel export**      | `ExcelExportService` powered by `ClosedXML` + a small `download.js` interop helper |

---

## 🧱 Stack

- **.NET 10** (SDK 10.0.x)
- **Blazor Server** with `@rendermode InteractiveServer`
- **Microsoft.FluentUI.AspNetCore.Components** (FluentUI Blazor)
- **ASP.NET Core Minimal API** (in-memory data store)
- **ClosedXML** for Excel export

---

## 🗂️ Project structure

```
src/
├── FluentDemo.Shared   →  DTOs / records shared between API and Web
├── FluentDemo.Api      →  ASP.NET Core Minimal API with in-memory data
└── FluentDemo.Web      →  Blazor Server UI (this is where the FluentUI demo lives)
```

Inside `FluentDemo.Web`:

```
Components/
├── App.razor                    → Root document
├── Routes.razor                 → Router
├── AuthGuard.razor              → Demo route guard
├── ChatBot.razor                → Floating assistant UI
├── Layout/
│   ├── MainLayout.razor         → Top bar, search palette, rail nav, popovers
│   ├── NavMenu.razor            → FluentNavMenu / NavGroup / NavLink
│   ├── EmptyLayout.razor        → Layout used by /login
│   ├── Breadcrumbs.razor        → FluentBreadcrumb wired to NavigationManager
│   └── NotifItem.razor          → Reusable notification row
└── Pages/
    ├── Login.razor
    ├── Home.razor               → Dashboard
    ├── Incentives.razor
    ├── CreateIncentive.razor    → Wizard
    ├── Reports.razor            → Charts + heatmap
    ├── Users.razor
    ├── UserEditDialog.razor
    ├── Resources.razor
    ├── ActivityLog.razor
    └── Settings.razor

Services/
├── AuthState.cs                 → Singleton fake auth state
├── LocalizationService.cs       → EN / TR strings
├── ThemeService.cs              → Mode + primary color
├── ExcelExportService.cs        → ClosedXML wrapper + JS interop
└── ChatBotService.cs            → Button-driven responses
```

---

## ▶️ Running locally

Requires **.NET 10 SDK**.

Open two terminals:

**Terminal 1 — API**

```powershell
dotnet run --project src/FluentDemo.Api
```

**Terminal 2 — Web**

```powershell
dotnet run --project src/FluentDemo.Web
```

Then open the URL printed by the Web project (default: <http://localhost:5159>).

### Demo credentials

| Field    | Value |
|----------|-------|
| Username | `admin` |
| Password | `123`   |

The Web app posts to `/api/auth/login` and the API just compares strings —
**no real authentication, no token, no cookie**. The signed-in state lives in
the singleton `AuthState` service inside the Blazor app only.

---

## 🎨 Using FluentUI Blazor — quick reference from this project

A few patterns that come back over and over again in the codebase:

### 1. Registering FluentUI

```csharp
// Program.cs
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

// Required at the root of the layout:
// <FluentToastProvider />
// <FluentDialogProvider />
// <FluentTooltipProvider />
// <FluentMessageBarProvider />
// <FluentMenuProvider />
```

### 2. Theming

```razor
<FluentDesignTheme @bind-Mode="_themeMode"
                   CustomColor="@Theme.PrimaryColor"
                   StorageName="fluentui-demo-theme" />
```

### 3. Data grid with template + property columns

```razor
<FluentDataGrid Items="@_rows.AsQueryable()" ShowHover="true"
                GridTemplateColumns="2fr 1fr 1.2fr 1fr">
    <PropertyColumn Property="@(x => x.FullName)" Title="Name" Sortable="true" />
    <PropertyColumn Property="@(x => x.Amount)"   Title="Amount" Format="N2" Align="Align.End" />
    <TemplateColumn Title="Status">
        <FluentBadge Appearance="Appearance.Neutral"
                     BackgroundColor="@StatusBg(context.Status)"
                     Color="@StatusFg(context.Status)">
            @context.Status
        </FluentBadge>
    </TemplateColumn>
</FluentDataGrid>
```

### 4. Popovers anchored to native buttons

```razor
<button id="btnNotif" class="topbar-iconbtn"
        @onclick="@(() => _open = !_open)">
    <FluentIcon Value="@(new Icons.Regular.Size20.Alert())" />
</button>

<FluentPopover AnchorId="btnNotif" @bind-Open="_open"
               HorizontalPosition="HorizontalPosition.Start">
    ...
</FluentPopover>
```

### 5. Toast + dialog services

```csharp
@inject IToastService Toast
@inject IDialogService Dialog

Toast.ShowSuccess("Saved");

var dlg = await Dialog.ShowDialogAsync<UserEditDialog>(
    model,
    new DialogParameters { Title = "Edit user" });
```

### 6. Excel export with ClosedXML

```csharp
await Excel.ExportAsync(
    $"users_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
    "Users",
    _filtered,
    ("Username",  x => x.Username),
    ("Full name", x => x.FullName),
    ("Email",     x => x.Email),
    ("Role",      x => x.Role));
```

### 7. Charts without an extra library

The Reports page draws **pie, donut, area, line and gauge** charts using plain SVG
helpers (`PiePath`, `DonutPath`, `AreaPath`, `LinePath`) computed in C# and rendered
inside `FluentCard`s. This keeps the dependency list minimal while still showing
that FluentUI plays nicely with custom visualizations.

---

## 🧪 Notes & limitations

- This is a **demo**. There is no persistence, no auth, no validation hardening.
- The API and Web ports may differ — check the console output.
- Some pages (Resources, Activity Log, Settings) use mocked client-side data only.
- The chat assistant is rule-based; there is no AI involved.

---

## 📜 License

MIT — feel free to fork, copy, and use as a starting point for your own
FluentUI Blazor experiments.
