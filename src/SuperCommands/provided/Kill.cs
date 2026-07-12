namespace SuperCommands.provided;

public sealed class Kill : Command
{
    public override string Name => "kill";
    public override string Description => "Kills a player.";
    public override string Usage => "/kill <player>";

    public override void Execute(string[] args)
    {
        if (args.Length == 0) throw CommandException.Usage(this);

        PlayerInfo player = Utils.GetPlayerByName(args[0]) ?? throw CommandException.PlayerNotFound(this, args[0]);

        player.AsGolfer.ServerEliminate(EliminationReason.OutOfBounds);
        Utils.SendMessage($"Killed {player.name}.");
    }
}