namespace SuperCommands.provided;

public sealed class Give : Command
{
    public override string Name => "give";
    public override string Description => "Give an item to a player";
    public override string Usage => "/give [player] <item> [amount] [-u]";

    public override void Execute(string[] args)
    {
        if (args.Length == 0) throw CommandException.Usage(this);

        PlayerInfo target = Utils.GetPlayerByName(args[0]) ?? throw CommandException.PlayerNotFound(this, args[0]);
        string itemArg = args[1];
        int amount;
        bool oneItem = args.Skip(3).Any(x => x == "-u");

        try
        {
            amount = int.Parse(args[2]);
        }
        catch
        {
            amount = 1;
        }

        ItemType? item = ParseItem(itemArg) ?? throw new CommandException(this, $"Invalid item \"{itemArg}\"");

        if (!GameManager.AllItems.TryGetItemData(item.Value, out var data)) throw new CommandException(this, $"Invalid item or not enough space!"); // no space or invalid item

        if (!oneItem)
        {
            for (int i = 0; i < amount; i++) target.Inventory.ServerTryAddItem(item.Value, data.MaxUses);
        }
        else
        {
            target.Inventory.ServerTryAddItem(item.Value, data.MaxUses);
        }

        string message;

        if (oneItem) message = $"Gave {itemArg} with {amount} usages to {target.name}."; else message = $"Gave {amount}x {itemArg} to {target.name}.";

        Utils.SendMessage(message);
    }

    public static ItemType? ParseItem(string name)
    {
        foreach (ItemType t in Enum.GetValues(typeof(ItemType)))
            if (t != ItemType.None && t.ToString().Equals(name, StringComparison.OrdinalIgnoreCase) || t.ToString().StartsWith(name, StringComparison.OrdinalIgnoreCase)) return t;

        return null;
    }

    public static void GiveItem(PlayerInfo player, ItemType item)
    {
        if (!GameManager.AllItems.TryGetItemData(item, out var data)) return;

        if (player.Inventory.HasSpaceForItem(out _)) player.Inventory.ServerTryAddItem(item, data.MaxUses);
    }

    public static void GiveItems(PlayerInfo player, string[] names)
    {
        foreach (var name in names)
        {
            ItemType? item = ParseItem(name);

            if (item.HasValue) GiveItem(player, item.Value);
        }
    }
}