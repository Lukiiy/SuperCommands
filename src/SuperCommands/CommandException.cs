namespace SuperCommands;

/// <summary>
/// Represents an exception that occurs during command execution.
/// </summary>
public sealed class CommandException : Exception
{
    public Command Command { get; }

    public CommandException(Command command, string message) : base(message) => Command = command;
    public CommandException(Command command, string message, Exception inner) : base(message, inner) => Command = command;
}