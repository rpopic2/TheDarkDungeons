public class Moveable : Entity
{
    public Position position {get; protected set;}

    public Moveable(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        position = new Position();
    }

    public virtual bool Move(int x)
    {
        Map current = Map.Current;
        Position newPos = position + x;
        bool success = current.Tiles.TryGet(newPos.x, out string? obj);
        if (!success) return false;

        position = newPos;
        current.UpdateMoveable(this);

        if (obj == MapSymb.portal)
        {
            Map.NewMap();
            position = new Position();
        }
        return true;
    }

}