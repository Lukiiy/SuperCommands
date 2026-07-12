using System.Reflection;
using HarmonyLib;

namespace SuperCommands.provided;

public sealed class ForceWin : Command
{
    public override string Name => "forcewin";
    public override string Description => "Force a win for the local player.";
    public override string Usage => "/forcewin [player]";

    private static readonly MethodInfo BallScoredMethod = AccessTools.Method(typeof(GolfHole), "ServerOnBallScored");
    private static readonly MethodInfo EnteredHoleMethod = AccessTools.Method(typeof(GolfBall), "ServerInformEnteredHole");

    public override void Execute(string[] args)
    {
        if (!GolfHoleManager.HasInstance || GolfHoleManager.MainHole == null) throw new CommandException(this, "There are no active golf holes.");

        PlayerInfo player = Utils.GetPlayerByName(args[0]) ?? throw CommandException.PlayerNotFound(this, args[0]);
        GolfBall ball = player.AsGolfer.OwnBall ?? throw new CommandException(this, $"Player {player.name} is not golfing.");

        EnteredHoleMethod.Invoke(ball, [GolfHoleManager.MainHole]);
        BallScoredMethod.Invoke(GolfHoleManager.MainHole, [ball]);
        Utils.SendMessage($"Forced {player.name} to score.");
    }
}