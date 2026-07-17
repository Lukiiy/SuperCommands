namespace SuperCommands.provided;

public sealed class Kill : Command
{
    public override string Name => "kill";
    public override string Description => "Kills a player.";
    public override string Usage => "/kill [player]";

    public override void Execute(string[] args)
    {
        PlayerInfo player = Utils.GetPlayerByName(args[0]) ?? GameManager.LocalPlayerInfo;

        player.AsGolfer.ServerEliminate(EliminationReason.OutOfBounds);
        Utils.SendMessage($"Killed {player.name}.");
    }
}