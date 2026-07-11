using HarmonyLib;
using Mirror;
using UnityEngine;

namespace SuperCommands;

public static class Utils
{
    private static readonly FastInvokeHandler RpcMsgFunction = MethodInvoker.GetHandler(AccessTools.Method(typeof(TextChatManager), "RpcMessage"));

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

    /// <summary>
    /// Sends a message the client's chat box.
    /// </summary>
    public static void SendMessage(string message) => TextChatUi.ShowMessage(message);

    /// <summary>
    /// Broadcasts a system message to all players.
    /// </summary>
    public static void BroadcastSystem(string message)
    {
        if (!SingletonNetworkBehaviour<TextChatManager>.HasInstance) return;
        if (!((NetworkBehaviour) (object) SingletonNetworkBehaviour<TextChatManager>.Instance).isServer) return;

        SendMessage(message); // host
        foreach (PlayerGolfer? player in CourseManager.ServerMatchParticipants ?? Enumerable.Empty<PlayerGolfer>())
        {
            if (player != null && !((NetworkBehaviour) (object) player).isLocalPlayer) RpcMsgFunction(SingletonNetworkBehaviour<TextChatManager>.Instance, $"[Broadcast] {message}", player.PlayerInfo);
        }
    }
}