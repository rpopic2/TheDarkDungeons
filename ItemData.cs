public readonly record struct ItemData(string abv, Action<Fightable>? onPickup, Action<Fightable>? onUse)
{
    public override string ToString()
    {
        return $"[{abv}]";
    }
}