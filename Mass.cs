public abstract class Mass : IMass
{
    public int level { get; protected set; }
    public int Sol { get; protected set; }
    public int Lun { get; protected set; }
    public int Con { get; protected set; }
    public abstract Card Draw();
}