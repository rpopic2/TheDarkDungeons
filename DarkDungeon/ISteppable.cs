public record struct Corpse(string name, Inventory droplist) : ISteppable
{
    public char ToChar() => MapSymb.corpse;
    public static Corpse operator +(Corpse old, Corpse newOne)
    {
        old.droplist.Content.AddRange(newOne.droplist);
        old.name += $", {newOne.name}";
        return old;
    }

    public void GetItemAndMeta(int index, out Item? item, out ItemMetaData? metaData)
    {
        item = droplist.ElementAtOrDefault(index);
        if(item is not null) metaData = droplist.GetMeta(item);
        else metaData = null;
    }
}
public record struct Pit() : ISteppable
{
    public string name { get; set; } = "구멍";
    public char ToChar() => MapSymb.portal;
}
public record struct Door() : ISteppable
{
    public string name { get; set; } = "갈래길";
    public char ToChar()=> MapSymb.door;
}

public interface IPortal
{

}
public interface ISteppable
{
    public char ToChar();
    public string name { get; set; }
}