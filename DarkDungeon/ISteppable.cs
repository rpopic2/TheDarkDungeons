public record Corpse(string name, List<Item?> droplist) : ISteppable
{
    public char ToChar() => MapSymb.corpse;
}
public record Portal(string name = "포탈") : ISteppable
{
    public char ToChar() => MapSymb.portal;
}

public interface ISteppable
{
    public char ToChar();
    public string name {get;}
}