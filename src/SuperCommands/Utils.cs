using UnityEngine;

namespace SuperCommands;

public static class Utils
{
    /// <summary>
    /// Finds a player by their name, performing case-insensitive matching.
    /// </summary>
    public static PlayerInfo? GetPlayerByName(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        PlayerInfo[] all = UnityEngine.Object.FindObjectsByType<PlayerInfo>(FindObjectsSortMode.None);

        PlayerInfo? exact = all.FirstOrDefault(p => p.PlayerId.PlayerNameNoRichText.Equals(input, StringComparison.OrdinalIgnoreCase));
        if (exact != null) return exact;

        IEnumerable<PlayerInfo> starts = all.Where(p => p.PlayerId.PlayerNameNoRichText.StartsWith(input, StringComparison.OrdinalIgnoreCase));
        if (starts.Any()) return starts.First();

        return null;
    }
}