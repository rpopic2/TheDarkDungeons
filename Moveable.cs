public class Moveable : Entity
{
    public int Position { get; private set; }
    private Facing facing;

    public Moveable(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
    }

    public virtual bool Move(int x)
    {
        Map? current = Map.Current;
        bool success = current.Content.TryGet(FrontPosition, out string? obj);
        if (!success) return false;
        facing = x < 0 ? Facing.Back : Facing.Front;
        current.entityContent[Position] = null;
        Position += x;
        current.entityContent[Position] = this;
        if (obj == MapSymb.portal)
        {
            current = null; //is this working?
            Map.NewMap();
            Position = 0;
        }
        return true;
    }
    public int FrontPosition
        => facing == Facing.Front ? Position + 1 : Position - 1;
}