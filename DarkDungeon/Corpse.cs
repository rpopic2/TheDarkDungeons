public record Corpse(string name, List<Item?> droplist)
{
    internal char ToChar() => MapSymb.corpse;
}
