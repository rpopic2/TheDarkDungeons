public record Torch : ItemEntity
{
    public static readonly ItemData data = new("TORCH", ItemType.Consum, null);
    public readonly Action<object?, EventArgs> onTurnEnd;

    public Torch(Inventoriable owner) : base(data, owner)
    {
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
    public override string ToString() => base.ToString();
}