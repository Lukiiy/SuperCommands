namespace SuperCommands;

public static class CommandAPI
{
    private static readonly Dictionary<string, Command> commands = [];

    public static void Register(Command command) => commands[command.Name.ToLower()] = command;

    public static bool TryHandle(string message)
    {
        if (!message.StartsWith('/')) return false;

        string[] parts = message[1..].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0) return true;
        if (!commands.TryGetValue(parts[0].ToLower(), out Command command)) return false;

        try
        {
            command.Execute(parts.Skip(1).ToArray());
        }
        catch (Exception e)
        {
            Utils.SendMessage($"An error occurred: {e.Message}");
            Plugin.Log.LogError($"An error occurred while executing \"/{command.Name}\".");
            Plugin.Log.LogError(e);
        }

        return true;
    }
}