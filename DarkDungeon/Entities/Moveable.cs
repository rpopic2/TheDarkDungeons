namespace Entities;
public class Moveable
{
    public int Level { get; protected set; }
    public readonly string Name;
    protected StanceInfo stance = new(default, default);
    public Moveable(int level, string name, Position pos)
    {
        Pos = pos;
        this.Level = level;
        Name = name;
    }
    public Position Pos { get; set; }

    public StanceInfo Stance => stance;
    protected virtual void Move(int x) => Move(x, out char obj);
    public virtual char ToChar() => Name.ToLower()[0];

    protected virtual bool Move(int x, out char obj)
    {
        Map current = Map.Current;
        Position newPos = Pos + x;
        bool existsTile = current.Tiles.TryGet(newPos.x, out obj);
        if (Pos.facing != newPos.facing)
        {
            Pos = !Pos;
            return true;
        }
        if (existsTile && current.steppables[newPos.x] is ISteppable step) obj = step.ToChar();
        bool obstructed = current.MoveablePositions.TryGet(newPos.x, out Moveable? mov);
        bool canGo = existsTile && !obstructed;
        if (canGo)
        {
            Pos = newPos;
            current.UpdateMoveable(this);
        }
        else
        {
            if (newPos.facing != Pos.facing)
            {
                Pos = !Pos;
                return true;
            }
            else
            {
                stance.Set(StanceName.None, default);
            }
        }
        return canGo;
    }
}