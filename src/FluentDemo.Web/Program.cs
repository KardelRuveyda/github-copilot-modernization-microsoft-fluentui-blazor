using FluentDemo.Web.Components;
using FluentDemo.Web.Services;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

builder.Services.AddSingleton<AuthState>();
builder.Services.AddScoped<LocalizationService>();
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<ChatBotService>();

builder.Services.AddHttpClient("Api", c =>
{
    var baseUrl = builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7001";
    c.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
