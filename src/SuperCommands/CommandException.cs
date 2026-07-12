namespace SuperCommands;

/// <summary>
/// Represents an exception that occurs during command execution.
/// </summary>
public sealed class CommandException : Exception
{
    public Command Command { get; }

    public static CommandException Usage(Command command) => new(command, $"Invalid usage. Try: {command.Usage}");
    public static CommandException PlayerNotFound(Command command, string playerName) => new(command, $"Player {playerName} not found.");

    public CommandException(Command command, string message) : base(message) => Command = command;
    public CommandException(Command command, string message, Exception inner) : base(message, inner) => Command = command;
}