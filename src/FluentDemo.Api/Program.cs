using FluentDemo.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

// ---- In-memory demo data --------------------------------------------------

var users = new List<AppUser>
{
    new() { Id = 1, Username = "admin",   FullName = "System Administrator", Email = "admin@fluentdemo.local",   Department = "IT",       Role = "Admin",   IsActive = true,  CreatedDate = new DateOnly(2024, 1, 10) },
    new() { Id = 2, Username = "mgr1",    FullName = "Mehmet Yılmaz",        Email = "mehmet@fluentdemo.local",  Department = "Sales",    Role = "Manager", IsActive = true,  CreatedDate = new DateOnly(2024, 2,  5) },
    new() { Id = 3, Username = "mgr2",    FullName = "Ayşe Demir",           Email = "ayse@fluentdemo.local",    Department = "Finance",  Role = "Manager", IsActive = true,  CreatedDate = new DateOnly(2024, 2, 18) },
    new() { Id = 4, Username = "user1",   FullName = "Ali Kaya",             Email = "ali@fluentdemo.local",     Department = "Sales",    Role = "User",    IsActive = true,  CreatedDate = new DateOnly(2024, 3,  9) },
    new() { Id = 5, Username = "user2",   FullName = "Zeynep Aksoy",         Email = "zeynep@fluentdemo.local",  Department = "HR",       Role = "User",    IsActive = true,  CreatedDate = new DateOnly(2024, 3, 22) },
    new() { Id = 6, Username = "user3",   FullName = "Can Öztürk",           Email = "can@fluentdemo.local",     Department = "IT",       Role = "User",    IsActive = false, CreatedDate = new DateOnly(2024, 4,  3) },
    new() { Id = 7, Username = "user4",   FullName = "Elif Şahin",           Email = "elif@fluentdemo.local",    Department = "Finance",  Role = "User",    IsActive = true,  CreatedDate = new DateOnly(2024, 5, 14) },
};
var nextUserId = users.Max(u => u.Id) + 1;

var depts = new[] { "Sales", "Finance", "HR", "IT", "Operations", "Marketing" };
var types = new[] { "Performance", "Sales Bonus", "Referral", "Project", "Spot Award" };
var statuses = new[] { "Approved", "Pending", "Rejected" };
var rng = new Random(42);

var incentives = Enumerable.Range(1, 60).Select(i =>
{
    var u = users[rng.Next(users.Count)];
    return new Incentive(
        Id: i,
        EmployeeNo: $"EMP{1000 + i}",
        FullName: u.FullName,
        Department: depts[rng.Next(depts.Length)],
        IncentiveType: types[rng.Next(types.Length)],
        Amount: Math.Round((decimal)(rng.NextDouble() * 9000 + 500), 2),
        Date: DateOnly.FromDateTime(DateTime.Today.AddDays(-rng.Next(0, 120))),
        Status: statuses[rng.Next(statuses.Length)]
    );
}).ToList();

// ---- Endpoints ------------------------------------------------------------

app.MapPost("/api/auth/login", (LoginRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
        return Results.Ok(new LoginResult(false, "Username and password are required."));

    var user = users.FirstOrDefault(u =>
        string.Equals(u.Username, req.Username, StringComparison.OrdinalIgnoreCase) && u.IsActive);

    // Demo: any non-empty password is accepted for an existing, active user.
    if (user is null)
        return Results.Ok(new LoginResult(false, "Invalid username or password."));

    return Results.Ok(new LoginResult(true, null));
});

app.MapGet("/api/incentives", () => Results.Ok(incentives));

app.MapGet("/api/users", () => Results.Ok(users));

app.MapPost("/api/users", (AppUser u) =>
{
    u.Id = nextUserId++;
    if (u.CreatedDate == default) u.CreatedDate = DateOnly.FromDateTime(DateTime.Today);
    users.Add(u);
    return Results.Created($"/api/users/{u.Id}", u);
});

app.MapPut("/api/users/{id:int}", (int id, AppUser u) =>
{
    var idx = users.FindIndex(x => x.Id == id);
    if (idx < 0) return Results.NotFound();
    u.Id = id;
    users[idx] = u;
    return Results.Ok(u);
});

app.MapDelete("/api/users/{id:int}", (int id) =>
{
    var removed = users.RemoveAll(x => x.Id == id);
    return removed > 0 ? Results.NoContent() : Results.NotFound();
});

app.Run();
