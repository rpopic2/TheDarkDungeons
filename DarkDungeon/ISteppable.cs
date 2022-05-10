public record struct Corpse(string name, Inventory droplist) : ISteppable
{
    public char ToChar() => MapSymb.corpse;
    public static Corpse operator +(Corpse old, Corpse newOne)
    {
        old.droplist.Content.AddRange(newOne.droplist);
        old.name += $", {newOne.name}";
        return old;
    }
}
public record struct Portal() : ISteppable
{
    public string name { get; set; } = "구멍";
    public char ToChar() => MapSymb.portal;
}

public interface ISteppable
{
    public char ToChar();
    public string name { get; set; }
}