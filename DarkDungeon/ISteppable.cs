public record struct Corpse(char was, string name, Inventory droplist) : ISteppable
{
    public static Corpse operator +(Corpse old, Corpse newOne)
    {
        foreach (ItemOld? item in newOne.droplist)
        {
            if(item is not null) old.droplist.Add(item);
        }
        old.name += $", {newOne.name}";
        return old;
    }

    public void GetItemAndMeta(int index, out ItemOld? item, out ItemMetaData? metaData)
    {
        item = droplist.ElementAtOrDefault(index);
        if(item is not null) metaData = droplist.GetMeta(item);
        else metaData = null;
    }

    public char ToChar() => MapSymb.corpse;
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
