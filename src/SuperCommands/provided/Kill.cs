namespace SuperCommands.provided;

public sealed class Kill : Command
{
    public override string Name => "kill";
    public override string Description => "Kills a player.";
    public override string Usage => "/kill <player>";

    public override void Execute(string[] args)
    {
        if (args.Length == 0) throw new CommandException(this, "Missing player name.");

        string target = string.Join(" ", args);

        PlayerInfo player = Utils.GetPlayerByName(target) ?? throw new CommandException(this, $"Player {target} not found.");

        player.AsGolfer.ServerEliminate(EliminationReason.OutOfBounds);
        Utils.SendMessage($"Killed {target}.");
    }
}