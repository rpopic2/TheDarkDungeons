public class Moveable : Entity
{
    public Position position;

    public Moveable(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        position = new Position();
    }

    public virtual bool Move(int x)
    {
        Map? current = Map.Current;
        Facing newFacing = x < 0 ? Facing.Back : Facing.Front;
        Position newPos = new Position(position.x, newFacing);
        bool success = current.Tiles.TryGet(newPos.Front, out string? obj);
        if (!success) return false;
        current.moveables[position.x] = null;
        position = new Position(position.x + x, newFacing);
        current.moveables[position.x] = this;
        if (obj == MapSymb.portal)
        {
            current = null; //is this working?
            Map.NewMap();
            position = new Position();
        }
        return true;
    }

}