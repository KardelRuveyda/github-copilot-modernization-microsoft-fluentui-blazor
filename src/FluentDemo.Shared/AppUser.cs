namespace FluentDemo.Shared;

public class AppUser
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Department { get; set; } = "";
    public string Role { get; set; } = "User";        // Admin / Manager / User
    public bool IsActive { get; set; } = true;
    public DateOnly CreatedDate { get; set; }
}
