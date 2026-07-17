namespace SuperCommands.provided;

public sealed class Clear : Command
{
    public override string Name => "clear";
    public override string Description => "Clears a player's inventory.";
    public override string Usage => "/clear [player]";

    public override void Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Handle(GameManager.LocalPlayerInfo);
            return;
        }

        Handle(Utils.GetPlayerByName(args[0]) ?? throw CommandException.PlayerNotFound(this, args[0]));
    }

    private static void Handle(PlayerInfo player)
    {
        PlayerInventory inventory = player.Inventory;

        for (int i = inventory.slots.Count - 1; i >= 0; i--)
            if (inventory.slots[i].itemType != ItemType.None) inventory.RemoveItemAt(i, false, false);

        Utils.SendMessage($"Cleared {player.name}'s inventory.");
    }
}