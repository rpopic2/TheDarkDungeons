namespace Entities;
public class Moveable : Entity
{
    protected StanceInfo stance = new(default, default);
    public Moveable(int level, int sol, int lun, int con, string name) : base(level, sol, lun, con, name)
    {
        Pos = new Position();
    }
    public Position Pos { get; set; }

    public StanceInfo CurStance => stance;
    public virtual void Move(int x) => Move(x, out char obj);
    public virtual char ToChar() => Name.ToLower()[0];

    protected virtual bool Move(int x, out char obj)
    {
        Map current = Map.Current;
        Position newPos = Pos + x;
        bool existsTile = current.Tiles.TryGet(newPos.x, out obj);
        bool obstructed = current.Moveables.TryGet(newPos.x, out Moveable? mov);
        bool result = existsTile && !obstructed;
        if (result)
        {
            stance = new(Stance.Move, default);
            Pos = newPos;
            current.UpdateMoveable(this);
        }
        else
        {
            if (newPos.facing != Pos.facing)
            {
                stance = new(Stance.Move, default);
                Pos = !Pos;
                return true;
            }
        }
        return result;
    }
}