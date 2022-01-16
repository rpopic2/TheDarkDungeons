public class Moveable : Entity
{
    public Position position { get; protected set; }

    public Moveable(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        position = new Position();
    }
    public virtual void Move(int x)
    {
        _Move(x, out string? obj);
    }

    protected virtual bool _Move(int x, out string? obj)
    {
        Map current = Map.Current;
        Position newPos = position + x;
        bool success = current.Tiles.TryGet(newPos.x, out obj);
        if (success)
        {
            position = newPos;
            current.UpdateMoveable(this);
        }
        return success;
    }

}