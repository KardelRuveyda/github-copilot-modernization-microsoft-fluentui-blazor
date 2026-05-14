namespace FluentDemo.Web.Services;

public class ChatBotService
{
    public record ChatMessage(bool FromBot, string Text, DateTime At, string[]? Suggestions = null, string? NavigateTo = null);

    private readonly List<ChatMessage> _messages = new();
    public IReadOnlyList<ChatMessage> Messages => _messages;
    public event Action? Changed;

    public ChatBotService()
    {
        Reset();
    }

    public void Reset()
    {
        _messages.Clear();
        _messages.Add(new ChatMessage(true,
            "👋 Hi! I'm FluentDemo Assistant. How can I help you today?",
            DateTime.Now,
            Suggestions: new[] {
                "Show incentives",
                "Create incentive",
                "View reports",
                "Manage users",
                "Open settings",
                "Help"
            }));
        Changed?.Invoke();
    }

    public ChatMessage Handle(string input)
    {
        var userMsg = new ChatMessage(false, input, DateTime.Now);
        _messages.Add(userMsg);

        var reply = BuildReply(input);
        _messages.Add(reply);
        Changed?.Invoke();
        return reply;
    }

    private static ChatMessage BuildReply(string input)
    {
        var t = input.Trim().ToLowerInvariant();

        if (t.Contains("incentive") && (t.Contains("show") || t.Contains("list") || t.Contains("view")))
            return new ChatMessage(true, "Opening the incentives list for you.", DateTime.Now,
                Suggestions: new[] { "Filter by status", "Export to Excel", "Go back" },
                NavigateTo: "/incentives");

        if (t.Contains("create") || t.Contains("new"))
            return new ChatMessage(true, "Let's create a new incentive together.", DateTime.Now,
                NavigateTo: "/create-incentive");

        if (t.Contains("report") || t.Contains("analytics") || t.Contains("chart"))
            return new ChatMessage(true, "Here are your reports and KPIs.", DateTime.Now,
                Suggestions: new[] { "Export to Excel", "Show by department", "Go back" },
                NavigateTo: "/reports");

        if (t.Contains("user"))
            return new ChatMessage(true, "Opening user management.", DateTime.Now,
                Suggestions: new[] { "Add user", "Export to Excel", "Go back" },
                NavigateTo: "/users");

        if (t.Contains("setting") || t.Contains("config") || t.Contains("theme"))
            return new ChatMessage(true, "Here are your settings.", DateTime.Now,
                NavigateTo: "/settings");

        if (t.Contains("activity") || t.Contains("audit") || t.Contains("log"))
            return new ChatMessage(true, "Showing the activity log.", DateTime.Now,
                NavigateTo: "/activity");

        if (t.Contains("resource"))
            return new ChatMessage(true, "Opening resources explorer.", DateTime.Now,
                NavigateTo: "/resources");

        if (t.Contains("export") || t.Contains("excel"))
            return new ChatMessage(true,
                "You can export any list to Excel using the ⬇ Export button in the command bar of Incentives, Users, Reports and Activity pages.",
                DateTime.Now,
                Suggestions: new[] { "Show incentives", "View reports" });

        if (t.Contains("help") || t.Contains("?"))
            return new ChatMessage(true,
                "I can help you navigate the portal: incentives, reports, users, settings, activity, create wizard, exports. Pick a suggestion or type a keyword.",
                DateTime.Now,
                Suggestions: new[] { "Show incentives", "Create incentive", "View reports", "Manage users" });

        if (t.Contains("hello") || t.Contains("hi") || t.Contains("hey"))
            return new ChatMessage(true, "Hello! 👋 What would you like to do?", DateTime.Now,
                Suggestions: new[] { "Show incentives", "Create incentive", "View reports" });

        return new ChatMessage(true,
            "I didn't understand that. Try one of the suggestions below.",
            DateTime.Now,
            Suggestions: new[] { "Show incentives", "Create incentive", "View reports", "Manage users", "Help" });
    }
}
