namespace FluentDemo.Web.Services;

public class AuthState
{
    public bool IsAuthenticated { get; private set; }
    public string? Username { get; private set; }

    public event Action? Changed;

    public void SignIn(string username)
    {
        IsAuthenticated = true;
        Username = username;
        Changed?.Invoke();
    }

    public void SignOut()
    {
        IsAuthenticated = false;
        Username = null;
        Changed?.Invoke();
    }
}
