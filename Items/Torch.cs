public record struct Torch : IItem
{
    public static readonly ItemData data = new("TORCH", ItemType.Consum, null);
    private readonly Inventoriable owner;
    public readonly Action<object?, EventArgs> onTurnEnd;
    public int stack { get; set; } = 1;
    public string abv { get; init; }
    public ItemType itemType { get; init; }
    public Action<Inventoriable>? onUse { get; init; }
    public Action<Inventoriable>? onExile { get; init; }

    public Torch(Inventoriable owner)
    {
        this.abv = data.abv;
        this.itemType = data.itemType;
        this.onUse = data.onUse;
        this.onExile = data.onExile;
        this.owner = owner;
        onUse = (f) =>
        {
            Player player = (Player)owner;
            player.torch = 20;
        };
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
        Program.OnTurnEnd -= torchHandler;
        Program.OnTurnEnd += torchHandler;
    }
    public override string ToString()
    {
        if (abv is null) return "[EMPTY]";
        return $"[{abv}{stack}]";
    }
}