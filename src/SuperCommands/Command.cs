namespace SuperCommands;

public abstract class Command
{
    public abstract string Name { get; }
    public virtual string Description => "No description.";
    public virtual string Usage => $"/{Name}";

    public abstract void Execute(string[] args);
}