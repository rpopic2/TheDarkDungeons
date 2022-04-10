namespace Entities;
public class Moveable : Entity
{
    protected StanceInfo stance = new(default, default);
    public Moveable(int level, string name, Position pos) : base(level, name)
    {
        Pos = pos;
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
        if(current.corpses[newPos.x] is char corpse) obj = corpse;
        bool obstructed = current.MoveablePositions.TryGet(newPos.x, out Moveable? mov);
        bool result = existsTile && !obstructed;
        if (result)
        {
            stance = new(StanceName.Charge, default);
            Pos = newPos;
            current.UpdateMoveable(this);
        }
        else
        {
            if (newPos.facing != Pos.facing)
            {
                stance = new(StanceName.Charge, default);
                Pos = !Pos;
                return true;
            }
        }
        return result;
    }
}