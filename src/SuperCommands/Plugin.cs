using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Mirror;

namespace SuperCommands;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;
    private Harmony harmony = null!;

    private void Awake()
    {
        harmony = new Harmony(Info.Metadata.GUID);
        Log = Logger;

        Log.LogInfo($"Mod {Name} loaded!");
        harmony.PatchAll();
        Commands.Register();
    }

    [HarmonyPatch(typeof(TextChatManager), nameof(TextChatManager.SendChatMessage))]
    internal static class Patch
    {
        [HarmonyPrefix]
        private static bool Prefix(string message)
        {
            if (!NetworkServer.active) return true;
            if (CommandAPI.TryHandle(message)) return false;

            return true;
        }
    }
}

