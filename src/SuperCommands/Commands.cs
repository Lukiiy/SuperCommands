namespace SuperCommands;

public static class Commands
{
    public static void Register()
    {
        CommandAPI.Register("kill", Kill);
    }

    private static void Kill(string[] args)
    {
        var player = FindPlayer(args);
        if (player == null) return;

        player.AsGolfer.ServerEliminate(EliminationReason.OutOfBounds);
    }

    public static PlayerInfo? FindPlayer(string[] args)
    {
        if (args.Length == 0)
        {
            Log("Missing player name.");
            return null;
        }

        string target = string.Join(" ", args);

        PlayerInfo player = UnityEngine.Object.FindObjectsByType<PlayerInfo>(UnityEngine.FindObjectsSortMode.None).FirstOrDefault(p => p.name.Equals(target, StringComparison.OrdinalIgnoreCase));
        if (player == null) Log($"Player '{target}' not found.");

        return player;
    }

    private static void Log(string msg) => Plugin.Log.LogInfo($"[CMD] {msg}");
}