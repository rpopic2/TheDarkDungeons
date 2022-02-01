public record Torch : ItemEntity
{
    public static readonly ItemData torch = new("TORCH", ItemType.Consum, null);
    public readonly Action<object?, EventArgs> onTurnEnd;
    public Torch(Inventoriable owner, Stat ownerStat) : base(torch, ownerStat)
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
        };
    }
    public override string ToString() => base.ToString();
}