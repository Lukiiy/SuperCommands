﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Mirror;
using SuperCommands.provided;

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

        // register provided ones
        CommandAPI.Register(new Kill());
        CommandAPI.Register(new Score());
        CommandAPI.Register(new ForceEnd());
        CommandAPI.Register(new Clear());
        CommandAPI.Register(new Give());
        CommandAPI.Register(new ForceWin());
        CommandAPI.Register(new BallTp());
    }

    [HarmonyPatch(typeof(TextChatManager), nameof(TextChatManager.SendChatMessage))]
    internal static class Patch
    {
        [HarmonyPrefix]
        private static bool Prefix(string message)
        {
            if (!NetworkServer.active) return true;

            return !CommandAPI.TryHandle(message);
        }
    }
}

