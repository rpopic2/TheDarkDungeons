public class Moveable : Entity
{
    public Position position;
    private Facing facing;

    public Moveable(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        position = new Position();
    }

    public virtual bool Move(int x)
    {
        Map? current = Map.Current;
        Facing newFacing = x < 0 ? Facing.Back : Facing.Front;
        bool success = current.Tiles.TryGet(position.FrontVal, out string? obj);
        if (!success) return false;
        facing = x < 0 ? Facing.Back : Facing.Front;
        current.moveables[position.val] = null;
        position = new Position(position.val + x, newFacing);
        current.moveables[position.val] = this;
        if (obj == MapSymb.portal)
        {
            current = null; //is this working?
            Map.NewMap();
            position = new Position();
        }
        return true;
    }

}