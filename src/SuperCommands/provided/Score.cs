namespace SuperCommands.provided;

public sealed class Score : Command
{
    public override string Name => "score";
    public override string Description => "Modify a player's score.";
    public override string Usage => "/score <player> <+/-> <amount>";

    public override void Execute(string[] args)
    {
        if (args.Length < 3) throw CommandException.Usage(this);

        bool add = args[1] == "+";
        int amount;

        try
        {
            amount = int.Parse(args[2]);
        }
        catch (FormatException)
        {
            throw new CommandException(this, "Invalid amount.");
        }

        PlayerInfo player = Utils.GetPlayerByName(args[0]) ?? throw CommandException.PlayerNotFound(this, args[0]);

        if (add) Add(player.AsGolfer, amount);
        else Add(player.AsGolfer, -amount);

        Utils.SendMessage($"Added {amount} to {args[0]}'s score.");
    }

    private static void Add(PlayerGolfer golfer, int increment)
    {
        if (!CourseManager.HasInstance) return;
        if (!CourseManager.Instance.TryGetPlayerStateIndex(golfer.connectionToClient, out int i)) return;

        var state = CourseManager.PlayerStates[i];

        state.matchScore += increment;
        state.courseScore += increment;
        CourseManager.PlayerStates[i] = state;

        BNetworkManager.singleton.ServerUpdateRules();
    }
}