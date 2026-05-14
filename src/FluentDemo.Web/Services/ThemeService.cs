namespace FluentDemo.Web.Services;

public class ThemeService
{
    public string Mode { get; private set; } = "light"; // light | dark | system
    public string PrimaryColor { get; private set; } = "#0078d4";
    public event Action? Changed;

    public void SetMode(string mode)
    {
        Mode = mode;
        Changed?.Invoke();
    }

    public void SetPrimary(string hex)
    {
        PrimaryColor = hex;
        Changed?.Invoke();
    }
}
