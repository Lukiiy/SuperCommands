namespace SuperCommands;

public static class CommandAPI
{
    private static readonly Dictionary<string, Action<string[]>> commands = [];

    public static void Register(string command, Action<string[]> handler) => commands[command.ToLower()] = handler;

    public static bool TryHandle(string message)
    {
        if (!message.StartsWith("/")) return false;

        string[] parts = message.TrimStart('/').Split(' ');
        string cmd = parts[0].ToLower();
        string[] args = parts.Length > 1 ? parts[1..] : [];

        if (commands.TryGetValue(cmd, out var handler))
        {
            handler(args);
            return true;
        }

        return false;
    }
}