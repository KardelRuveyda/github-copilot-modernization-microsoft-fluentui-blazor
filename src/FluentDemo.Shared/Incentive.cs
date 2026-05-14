namespace FluentDemo.Shared;

public record Incentive(
    int Id,
    string EmployeeNo,
    string FullName,
    string Department,
    string IncentiveType,
    decimal Amount,
    DateOnly Date,
    string Status   // Approved / Pending / Rejected
);

public record LoginRequest(string Username, string Password);

public record LoginResult(bool Success, string? Message);
