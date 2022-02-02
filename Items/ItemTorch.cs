public readonly record struct TorchData : IItemData
{
    public static TorchData data = new();
    public string abv { get; init; } = "TORCH";
    public ItemType itemType { get; init; } = ItemType.Consum;
    public Func<Inventoriable, bool>? onUse { get; init; }

    public IItem Instantiate(Inventoriable owner, Stat ownerStat)
    {
        return new Torch(owner, ownerStat);
    }
}

public record Torch : Item
{
    public readonly Action<object?, EventArgs> onTurnEnd;
    public Torch(Inventoriable owner, Stat ownerStat) : base(new TorchData(), ownerStat)
    {
        onTurnEnd = (object? sender, EventArgs e) =>
        {
            Player player = (Player)owner;
            if (player.torch > 0)
            {
                player.sight = 3;
                player.torch--;
                if (player.torch <= 0)
                {
                    player.sight = 1;
                }
            }
        };
        EventHandler torchHandler = new(onTurnEnd);
        onUse = (f) =>
        {
            Player player = (Player)owner;
            player.torch = 20;
            Program.OnTurnEnd -= torchHandler;
            Program.OnTurnEnd += torchHandler;
            return true;
        };
    }
    public override string ToString() => base.ToString();
    public static new IItem Instantiate(Inventoriable owner, Stat ownerStat, IItemData data)
    {
        return new Torch(owner, ownerStat);
    }
}