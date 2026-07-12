namespace SuperCommands.provided;

public sealed class ForceEnd : Command
{
    public override string Name => "forceend";
    public override string Description => "Force the end of the current round.";
    public override string Usage => "/forceend";

    public override void Execute(string[] args)
    {
        if (CourseManager.HasInstance)
        {
            CourseManager.Instance.NetworkmatchState = MatchState.Ended;
            Utils.SendMessage("Round ended forcefully.");
            return;
        }

        Utils.SendMessage("No active rounds found.");
    }
}