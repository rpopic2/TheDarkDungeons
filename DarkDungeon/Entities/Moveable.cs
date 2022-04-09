namespace Entities;
public class Moveable : Entity
{
    protected StanceInfo stance = new(default, default);
    public Moveable(int level, int sol, int lun, int con, string name) : base(level, sol, lun, con, name)
    {
        Pos = new Position();
    }
    public Position Pos { get; set; }

    public StanceInfo Stance => stance;
    public virtual void Move(int x) => Move(x, out char obj);
    public virtual char ToChar() => Name.ToLower()[0];

    protected virtual bool Move(int x, out char obj)
    {
        Map current = Map.Current;
        Position newPos = Pos + x;
        bool existsTile = current.Tiles.TryGet(newPos.x, out obj);
        bool obstructed = current.MoveablePositions.TryGet(newPos.x, out Moveable? mov);
        bool result = existsTile && !obstructed;
        if (result)
        {
            stance = new(global::StanceName.Charge, default);
            Pos = newPos;
            current.UpdateMoveable(this);
        }
        else
        {
            if (newPos.facing != Pos.facing)
            {
                stance = new(global::StanceName.Charge, default);
                Pos = !Pos;
                return true;
            }
        }
        return result;
    }
}