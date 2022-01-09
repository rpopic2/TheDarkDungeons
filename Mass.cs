public abstract class Mass : IMass
{
    public int sol {get; protected set;}
    public int lun {get; protected set;}
    public int con {get; protected set;}
    public abstract Card Draw();
}