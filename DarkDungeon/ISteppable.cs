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
public record struct Pit() : ISteppable, IPortal
{
    public string name { get; set; } = "구멍";
    public char ToChar() => MapSymb.pit;
}
public record struct Door() : ISteppable, IPortal
{
    public string name { get; set; } = "갈래길";
    public char ToChar()=> MapSymb.door;
}
public record struct RandomPortal() : ISteppable, IPortal
{
    public string name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public char ToChar()
    {
        throw new NotImplementedException();
    }
}

public interface IPortal : ISteppable
{

}
public interface ISteppable
{
    public char ToChar();
    public string name { get; set; }
}